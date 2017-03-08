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
                GuiChanges.Show(chkBorrowed);
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
                GuiChanges.Hide(chkBorrowed);
            }
        }

        public void UpdateFromItem()
        {
            UIElement[] issue = { lblIssue, txtIssue };
            UIElement[] author = { lblAuthor, txtAuthor };
            lblISBN.Text = $"ISBN: {CurrentItem.ISBN}";
            lblTypeOf.Text = CurrentItem.ItemType;
            chkBorrowed.IsChecked = CurrentItem.IsBorrowed;
            GuiChanges.FillComboWithBaseCategory(cmbBaseCat);
            cmbBaseCat.SelectedItem = CurrentItem.BaseCategory;
            txtName.Text = CurrentItem.Name;
            dtPick.SelectedDate = CurrentItem.PrintDate;
            switch (CurrentItem.ItemType)
            {
                case "Book":
                    GuiChanges.Hide(issue);
                    GuiChanges.Show(author);
                    txtAuthor.Text = ((Book)CurrentItem).Author;
                    break;
                case "Journal":
                    GuiChanges.Hide(author);
                    GuiChanges.Show(issue);
                    txtIssue.Text = ((Journal)CurrentItem).IssueNumber.ToString();
                    break;
            }
        }

        private void FillInnerCategory()
        {
            GuiChanges.FillComboWithInnerCategory(cmbInnerCat, cmbBaseCat.SelectedItem);
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
                if (FieldsValidForUpdate())
                {
                    UpdateCurrentItem();
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void UpdateCurrentItem()
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
                    ((Journal)CurrentItem).IssueNumber = int.Parse(txtIssue.Text);
                    break;
            }
        }

        /// <summary>
        /// Check that all the fields are valid before Update the Current Item
        /// </summary>
        /// <returns>true if the fields are OK and false if opposite</returns>
        private bool FieldsValidForUpdate()
        {
            if (!Validity.StringOK(txtName.Text))
            {
                GuiMsgs.Warning("Please enter the Item Name!");
                return false;
            }
            else if (dtPick.SelectedDate == null)
            {
                GuiMsgs.Warning("Please Select the Publishing Date!");
                return false;
            }
            else
            {
                switch (CurrentItem.ItemType)
                {
                    case "Book":
                        if (!Validity.StringOK(txtAuthor.Text))
                        {
                            GuiMsgs.Warning("Please Enter the Author Name!");
                            return false;
                        }
                        break;
                    case "Journal":
                        if (!Validity.StringOK(txtIssue.Text))
                        {
                            GuiMsgs.Warning("Please Enter The Issue Number!");
                            return false;
                        }
                        else if (!Validity.PositiveInteger(txtIssue.Text))
                        {
                            GuiMsgs.Warning("Please Enter the valid Issue Number!");
                            return false;
                        }
                        break;
                }
            }
            return true;
        }
    }
}
