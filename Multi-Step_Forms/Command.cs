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
            MyForm currentForm = new MyForm("", doc, null)
            {
                Width = 750,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.ShowDialog();

            // get form data and do something
            //List<Reference> refList = new List<Reference>();
            List<string> refList = new List<string>();
            bool flag = true;

            while (flag == true)
            {
                // while loop will continue as long as the user continues selecting elements and does not hit the escape key
                try
                {
                    // Ask the user to select elements
                    Reference curRef = uidoc.Selection.PickObject(ObjectType.Element, "Pick an item");
                    
                    // filter out anything that is not a viewport selection
                    Element element = doc.GetElement(curRef);
                    if (element.Category != null && element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Viewports)
                    {
                        // if element is a viewport, add it to refList
                        refList.Add(curRef.ElementId.ToString());
                    }

                    
                }
                catch (Exception)
                {
                    // if the user hit escape, set the flag to false to exit the selection loop
                    flag = false;
                }
            }

            string returnString = "There are " + refList.Count.ToString() + " selected elements";
            List<string> returnStrings = currentForm.GetSelectedListboxItems();

            //MyForm currentForm2 = new MyForm(returnString, doc, returnStrings)
            MyForm currentForm2 = new MyForm(returnString, doc, refList)
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
                string returnString2 = currentForm2.GetSelectedComboboxItem();
                List<string> returnStrings2 = currentForm2.GetSelectedListboxItems();
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
