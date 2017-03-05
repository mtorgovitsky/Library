using BL;
using BL.Modules;
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

namespace Library
{
    /// <summary>
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditItem : Window
    {
        public static AbstractItem CurrentItem;
        public static bool EditMode { get; set; }

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
            if (!EditMode)
            {
                this.Title = "Item Details";
                foreach (UIElement item in this.grdWindowGrid.Children)
                {
                    GuiChanges.Disable(item);
                }
                btnSaveExit.Content = "Exit";
                GuiChanges.Enable(btnSaveExit);
            }
            else
            {
                btnSaveExit.Content = "Save Changes";
            }
        }

        public void UpdateFromItem()
        {
            lblISBN.Text = $"ISBN: {CurrentItem.ISBN}";
            lblTypeOf.Text = CurrentItem.GetType().Name;
            cmbBaseCat.ItemsSource = Enum.GetValues(typeof(Categories.eBaseCategory));
            cmbBaseCat.SelectedItem = CurrentItem.BaseCategory;
            txtName.Text = CurrentItem.Name;
            dtPick.SelectedDate = CurrentItem.PrintDate;
            switch (CurrentItem.ItemType)
            {
                case "Book":
                    GuiChanges.Hide(lblIssue, txtIssue);
                    GuiChanges.Show(lblAuthor, txtAuthor);
                    txtAuthor.Text = ((Book)CurrentItem).Author;
                    break;
                case "Journal":
                    GuiChanges.Hide(lblAuthor, txtAuthor);
                    GuiChanges.Show(lblIssue, txtIssue);
                    txtIssue.Text = ((Journal)CurrentItem).IssueNumber.ToString();
                    break;
            }
        }

        private void FillInnerCategory()
        {
            cmbInnerCat.ItemsSource = Categories.CategoriesDictionary[(Categories.eBaseCategory)cmbBaseCat.SelectedItem];
            cmbInnerCat.SelectedItem = CurrentItem.InnerCategory;
            if (cmbInnerCat.SelectedIndex < 0)
                cmbInnerCat.SelectedIndex++;
        }

        private void cmbBaseCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillInnerCategory();
        }

        private void btnSaveExit_Click(object sender, RoutedEventArgs e)
        {
            if (EditMode)
            {
                CurrentItem.Name = txtName.Text;
                CurrentItem.BaseCategory = (Categories.eBaseCategory)cmbBaseCat.SelectedItem;
                CurrentItem.InnerCategory = (Categories.eInnerCategory)cmbInnerCat.SelectedItem;
                CurrentItem.PrintDate = dtPick.SelectedDate.Value;
                switch (CurrentItem.ItemType)
                {
                    case "Book":
                        ((Book)CurrentItem).Author = txtAuthor.Text;
                        break;
                    case "Journal":
                        break;
                }
            }
            else
            {
                this.Close();
            }
        }

        private bool FieldsValid()
        {
            if (!Validity.StringOK(txtName.Text))
            {
                GuiMsgs.Warning("The Name is Required!");
                return false;
            }
            else if (dtPick.SelectedDate == null)
            {
                GuiMsgs.Warning("The Date is Required!");
                return false;
            }
            return true;
        }
    }
}
