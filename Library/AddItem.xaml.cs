using BL;
using BookLib;
using Library.Modules;
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
using System.Windows.Shapes;

namespace Library.GUI
{
    /// <summary>
    /// Interaction logic for AddNewItem.xaml
    /// </summary>
    public partial class AddNewItem : Window
    {
        public AddNewItem()
        {
            InitializeComponent();
            //cmbItemType.ItemsSource = new List<string>() { "Book", "Journal" };
            //cmbItemType.SelectedIndex = 0;
            GuiChanges.Hide(lblIssue, lblAuthor, txtIssue, txtAuthor);
            cmbBaseCat.ItemsSource = Enum.GetValues(typeof(Categories.eBaseCategory));
            GuiChanges.Disable(cmbInnerCat);
        }

        private void FillInnerCombo(object sender, SelectionChangedEventArgs e)
        {
            GuiChanges.Enable(cmbInnerCat);
            cmbInnerCat.ItemsSource = Categories.CategoriesDictionary[(Categories.eBaseCategory)cmbBaseCat.SelectedItem];
        }

        private void rdChecked(object sender, RoutedEventArgs e)
        {
            var rdValue = sender as RadioButton;
            switch (rdValue.Name)
            {
                case "rdBook":
                    GuiChanges.Hide(lblIssue, txtIssue);
                    GuiChanges.Show(lblAuthor, txtAuthor);
                    break;
                case "rdJournal":
                    GuiChanges.Hide(lblAuthor, txtAuthor);
                    GuiChanges.Show(lblIssue, txtIssue);
                    break;
            }
        }
    }
}
