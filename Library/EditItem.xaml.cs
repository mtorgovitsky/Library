using BL;
using BookLib;
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

namespace Library
{
    /// <summary>
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditItem : Window
    {
        public static AbstractItem Item;

        public EditItem()
        {
            InitializeComponent();
            InitEditItemWindow();
            UpdateFromItem();
        }

        private void InitEditItemWindow()
        {
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lblISBN.TextAlignment = TextAlignment.Center;
        }

        public void UpdateFromItem()
        {
            lblISBN.Text = $"ISBN: {Item.ISBN}";
            lblTypeOf.Text = Item.GetType().Name;
            cmbBaseCat.ItemsSource = Enum.GetValues(typeof(Categories.eBaseCategory));
            cmbBaseCat.SelectedItem = Item.BaseCategory;
        }

        private void FillInner()
        {
            cmbInnerCat.ItemsSource = Categories.CategoriesDictionary[(Categories.eBaseCategory)cmbBaseCat.SelectedItem];
            cmbInnerCat.SelectedItem = Item.InnerCategory;
            if (cmbInnerCat.SelectedIndex < 0)
                cmbInnerCat.SelectedIndex++;
        }

        private void cmbBaseCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillInner();
        }
    }
}
