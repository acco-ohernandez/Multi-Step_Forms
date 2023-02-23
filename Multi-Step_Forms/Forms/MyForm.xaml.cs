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
        public MyForm(string testText, Document doc, List<string> listBoxItems)
        {
            InitializeComponent();
            myDoc = doc;
            lbxElemIds.Items.Clear();
            cmbViewsList.Items.Clear();

            if (testText == "" && listBoxItems == null)
                PopulateControls();
            else
            {
                //lblLabel.Content = testText + doc.PathName;

                foreach (string item in listBoxItems)
                {
                    //lbxElemIds.Items.Add(item);
                    lbxElemIds.Items.Add(item);
                    cmbViewsList.Items.Add(item);
                }

                //cmbViewsList.Items.Add(testText);
            }

            cmbViewsList.SelectedIndex = 0;
        }

        public void PopulateControls()
        {
            FilteredElementCollector collector = new FilteredElementCollector(myDoc);
            collector.OfCategory(BuiltInCategory.OST_Views);
            collector.WhereElementIsNotElementType();

            foreach (View currentView in collector)
            {
                lbxElemIds.Items.Add(currentView.Name);
                
            }

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

        internal List<string> GetSelectedListboxItems()
        {
            List<string> returnList = new List<string>();

            foreach (var item in lbxElemIds.SelectedItems)
            {
                returnList.Add(item.ToString());
            }

            return returnList;
        }

        internal string GetSelectedComboboxItem()
        {
            return cmbViewsList.SelectedItem.ToString();
        }

        private void cmbViewsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string startItem = cmbViewsList.SelectedItem.ToString();
            lblSelected.Content = "Reorder will start with: " + startItem;
        }
    }
}