#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
//using System.Windows;
#endregion

namespace Multi_Step_Forms
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // put any code needed for the form here

            // open form
            MyForm currentForm = new MyForm(doc, null)
            {
                Width = 750,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.ShowDialog();

            // get form data and do something
            //List<Reference> refList = new List<Reference>();
            List<Reference> refList = new List<Reference>();
            bool flag = true;

            while (flag == true)
            {
                // while loop will continue as long as the user continues selecting elements and does not hit the escape key
                try
                {
                    // Ask the user to select elements
                    Reference curRef = uidoc.Selection.PickObject(ObjectType.Element, "Pick the viewports you want to renumber, then hit ESC.");
                    
                    // filter out anything that is not a viewport selection
                    Element element = doc.GetElement(curRef);
                    if (element.Category != null && element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Viewports)
                    {
                        // if element is a viewport, add it to refList
                        refList.Add(curRef);
                    }

                    
                }
                catch (Exception)
                {
                    // if the user hit escape, set the flag to false to exit the selection loop
                    flag = false;
                }
            }

            //string returnString = "There are " + refList.Count.ToString() + " selected elements";
            //List<Element> selectedListBoxItems = currentForm.GetSelectedListboxItems();

            //MyForm currentForm2 = new MyForm(returnString, doc, returnStrings)
            MyForm currentForm2 = new MyForm(doc, refList)
            {
                Width = 750,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm2.ShowDialog();

            // What to do if user clicked Ok.
            if (currentForm2.DialogResult == true)
            {
                int startNumber = currentForm2.GetSelectedComboboxItem();
                int tempNum = startNumber;
                int counter = 0;
                List<Element> returnElements = currentForm2.GetSelectedListboxItems();
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Renumber Viewports");

                    foreach (Element curElem in returnElements)
                    {
                        Parameter curParam = curElem.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                        curParam.Set("zzzzz" + tempNum.ToString());
                        tempNum++;
                    }

                    tempNum = startNumber;

                    foreach (Element curElem2 in returnElements)
                    {
                        Parameter curParam = curElem2.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                        curParam.Set(tempNum.ToString());

                        tempNum++;
                        counter++;
                    }

                    t.Commit();
                }

            }
            

            //System.Threading.Thread.Sleep(1000);

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
