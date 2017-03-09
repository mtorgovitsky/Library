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
        //Current Item to deal with in the current window
        public static AbstractItem CurrentItem;

        //If updating existing or just showing the details
        public static bool EditMode { get; set; }

        //Ctor for window
        public EditItem()
        {
            InitializeComponent();
            //init all elements depending on current working state
            InitEditItemWindow();
            //update the fields with Item's details
            UpdateFromItem();
        }

        //init all elements depending on current working state
        private void InitEditItemWindow()
        {
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lblISBN.TextAlignment = TextAlignment.Center;
            if (!EditMode) //showing details mode
            {
                this.Title = "Item Details";
                GuiChanges.Show(chkBorrowed);
                foreach (UIElement item in this.grdWindowGrid.Children)
                {
                    GuiChanges.Disable(item);
                }
                btnSaveExit.Content = "Exit";
                //After all controls being disabled because of "SHow Details Mode",
                //Enable the Exit button
                GuiChanges.Enable(btnSaveExit);
            }
            else //editing Item Mode
            {
                btnSaveExit.Content = "Save Changes";
                //Hide "Item is Borrowed" indicator CheckBox
                GuiChanges.Hide(chkBorrowed);
            }
        }

        //Update the fields from current Item
        public void UpdateFromItem()
        {
            //creating the arrays for easy handling the controls
            UIElement[] issue = { lblIssue, txtIssue };
            UIElement[] author = { lblAuthor, txtAuthor };

            //updating the fields
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

        //Fill the Inner Category ComboBox with relative values
        private void FillInnerCategory()
        {
            GuiChanges.FillComboWithInnerCategory(cmbInnerCat, cmbBaseCat.SelectedItem);
            cmbInnerCat.SelectedItem = CurrentItem.InnerCategory;
            if (cmbInnerCat.SelectedIndex < 0)
                cmbInnerCat.SelectedIndex++;
        }

        //Event to handle in Base Category ComboBox
        private void cmbBaseCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillInnerCategory();
        }

        //Saving after editing or just Exit if Showing details
        private void btnSaveExit_Click(object sender, RoutedEventArgs e)
        {
            if (EditMode) //Update current Item if we are in the Editing Mode
            {
                if (FieldsValidForUpdate()) //If all of the values are OK
                {
                    UpdateCurrentItem(); //Make an Update
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        //Updates the Current Item
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
        /// Validation method that checks if all the fields are valid
        /// before Update the Current Item
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
