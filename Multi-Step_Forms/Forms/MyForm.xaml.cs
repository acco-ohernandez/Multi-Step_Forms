using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Multi_Step_Forms
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        public Document myDoc;
        public List<Element> elemList;
        public MyForm(Document doc, List<Reference> listBoxItems)
        {
            InitializeComponent();
            myDoc = doc;
            lbxElemIds.Items.Clear();
            cmbViewsList.Items.Clear();

            if (listBoxItems == null)
                PopulateControls();
            else
            {
                //lblLabel.Content = testText + doc.PathName;
                lbxElemIds.Items.Clear();
                elemList = new List<Element>();

                foreach (Reference curRef in listBoxItems)
                {
                    //lbxElemIds.Items.Add(item);

                    Element curElem = doc.GetElement(curRef);

                    if (curElem is Viewport)
                    {
                        elemList.Add(curElem);
                        //Viewport curVP = curElem as Viewport;
                        //View curView = doc.GetElement(curVP.ViewId) as View;

                        Parameter curParam = curElem.get_Parameter(BuiltInParameter.VIEWPORT_VIEW_NAME);
                        Parameter curParam2 = curElem.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);

                        lbxElemIds.Items.Add(curParam2.AsString() + ": " + curParam.AsString() + " (" + curElem.Id.ToString() + ")");
                        //lbxElements.Items.Add(curElem.Id.ToString());
                    }
                }

                // add 1..20 to the cmbViowsList
                for (int i = 1; i <=20 ; i++)
                {
                    cmbViewsList.Items.Add(i);
                }


                //cmbViewsList.Items.Add(testText);
            }

            cmbViewsList.SelectedIndex = 0;
        }

        public void PopulateControls()
        {
            lbxElemIds.Items.Add("Click Select to pic your viewports");
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            this.DialogResult = true;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        internal List<Element> GetSelectedListboxItems()
        {
            if (elemList != null)
                return elemList;
            else
                return null;
        }

        internal int GetSelectedComboboxItem()
        {
            string cmbViewsListNumber =  cmbViewsList.SelectedItem.ToString();
            int selectedNumber = Convert.ToInt32(cmbViewsListNumber);
            return selectedNumber;

        }

        private void cmbViewsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbViewsList.SelectedItem != null)
            {
                string startItem = cmbViewsList.SelectedItem.ToString();
                lblSelected.Content = "Reorder will start with: " + startItem; 
            }
        }
    }
}