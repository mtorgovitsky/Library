using BL;
using BL.Modules;
using BookLib;
using Data;
using Library.GUI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static BL.Categories;

namespace Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Main Library Class which contains all the data
        public static ItemsCollection mainLibrary = new ItemsCollection();

        //Flag variable to know if there's mulisearch mode or
        //just regular search mode turned on
        public bool IsMultiSearch { get; set; }

        //Enum for easy manage of Showing or hiding the controls
        public enum eShowOrHide
        {
            Show,
            Hide
        }

        public MainWindow()
        {
            InitializeComponent();
            
            //User must login first
            var login = new LoginWindow();
            login.ShowDialog();
            
            //Init all the controls after login
            InitMainWindow();
        }

        private void InitMainWindow()
        {
            //The name of the method speaks for itself
            RefreshDataGrid();
            
            //Hide the search fields(controls)
            ShowAndHideSearchFields(eShowOrHide.Hide);
            
            //Prevent the inplace editing
            dataLib.IsReadOnly = true;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Close the App
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Show the Add Item dialog
            var addItem = new AddNewItem();
            addItem.ShowDialog();
            
            //After Add Item dialog has been closed - refresh DataGrid
            RefreshDataGrid();
            
            //Also cancel the search results 
            //because the collection of Items has changed
            UntickSearchOptions();
        }

        private void ClosingMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Serialize all the data on exit - 
            //save the whole Library to Data File
            mainLibrary.SaveData(mainLibrary);
        }

        public void RefreshDataGrid()
        {
            //Referring to main library again to show all the Data
            dataLib.ItemsSource = mainLibrary.Items;
            
            //Refreshing the source
            dataLib.Items.Refresh();
            
            //If the selection made - change the controls in relative manner
            GridSelected();
        }
        
        //Button "Edit" event
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //We're in editing mode
            EditItem.EditMode = true;
            
            //The Item to edit inserted to Edit/Details Dialog variable
            EditItem.CurrentItem = (AbstractItem)dataLib.SelectedItem;
            
            //Show the Edit Dialog
            var editW = new EditItem();
            editW.ShowDialog();
            
            //After closing the Editor - refresh the Data Grid
            RefreshDataGrid();
            
            //Unticking the search because there's possible changes
            //will be made to the library Item
            UntickSearchOptions();
        }

        /// <summary>
        /// Manipulation with the GUI Elements depending on selection that user made
        /// </summary>
        /// <returns>Selected Item Index if it's in valid range</returns>
        public int GridSelected()
        {
            //Creating the Array of elements for simple handling of GUI,
            //instead of passing to GuiChanges class Methods each one of the controls,
            //pass only one array variable. Simple and Effective
            UIElement[] controlsForDataGrid = { btnEdit, btnDetails, btnDelete, btnBorrow, btnQuantity };

            //if user clicks in the DataGrid - check if the click Event
            //Occurred in the valid range to prevent Exception
            if (dataLib.SelectedIndex >= 0 && dataLib.SelectedIndex < mainLibrary.Items.Count)
            {
                //Enable the buttons
                GuiChanges.Enable(controlsForDataGrid);
                
                //Fine tuning the controls depending on current user Rights
                ButtonsAvailable();
                
                //Temp variable for Changing the controls text
                var tmpItem = (AbstractItem)dataLib.SelectedItem;

                //------------    Changing the controls Caption depending   ------------//
                //------------ on the current Item choosed from the Library ------------//
                btnDelete.Content = $"Delete this {tmpItem.ItemType}";
                btnDetails.Content = $"Details of this {tmpItem.ItemType}";
                btnEdit.Content = $"Edit this {tmpItem.ItemType}";
                btnQuantity.Content = $"Quantity of this {tmpItem.ItemType}";
                //------------    Changing the controls Caption depending   ------------//
                //------------ on the current Item choosed from the Library ------------//

                //Switch for Borrowing Button - depends on current Item "IsBorrowed" State
                switch (tmpItem.IsBorrowed)
                {
                    case true:
                        btnBorrow.Content = $"Return this {tmpItem.ItemType}";
                        break;
                    case false:
                        btnBorrow.Content = $"Borrow this {tmpItem.ItemType}";
                        break;
                }
                
                //Return Selected Item index
                return dataLib.SelectedIndex;
            }
            //If the Selected Index is in invalid range
            else
            {
                //Disable the related controls
                GuiChanges.Disable(controlsForDataGrid);
                
                //Fine tuning the controls depending on current user Rights
                ButtonsAvailable();

                //Return the index (even tough it's invalid)
                return dataLib.SelectedIndex;
            }
        }

        //Selection change event fired - 
        //check if the Selection is valid and make changes to Elements if so
        private void dataLib_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridSelected();
        }

        //Button "Details" event
        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            //Show Details mode, editing is not allowed
            EditItem.EditMode = false;
            
            //The Item to edit inserted to Details/Edit Dialog variable
            EditItem.CurrentItem = (AbstractItem)dataLib.SelectedItem;
            
            //Show the Details Dialog
            var tmpW = new EditItem();
            tmpW.ShowDialog();
        }

        //Delete Button Clicked Event
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Last chance to regret before deleting the choosed Item
            if (GuiMsgs.AreYouSure(
                "Are You Positive that You want to delete this Item?" +
                "\n(There's no way You can undo this action!)"))
            {
                //If Uses Approves the deletion - remove the Item from the Library
                mainLibrary.Items.Remove((AbstractItem)dataLib.SelectedItem);

                //Refresh DataGrid because changes was made
                RefreshDataGrid();

                //Cancel the search because changes was made
                UntickSearchOptions();
            }
        }

        //Fine tuning the controls depending on current user Rights
        private void ButtonsAvailable()
        {
            //Make desision depending on current User rights - 
            //Show and hide relative Controls
            switch (mainLibrary.LibraryUsers.CurrentUser.Type)
            {
                case User.eUserType.Employee:
                    GuiChanges.Disable(btnUsers);
                    break;
                case User.eUserType.Client:
                    GuiChanges.Disable(btnDelete, btnUsers, btnEdit, btnAdd, btnBorrow, btnQuantity);
                    break;
            }
        }

        //Edit Users Button Click Event
        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            //Show the Edit Users Dialog
            var editUsers = new EditUsers();
            editUsers.ShowDialog();
        }

        //Borrow The Item Button Click Event
        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            //Temp variable for selected Item
            AbstractItem choosenItem = (AbstractItem)dataLib.SelectedItem;

            //Change the state of IsBorrowed to opposite
            choosenItem.IsBorrowed = !choosenItem.IsBorrowed;

            //Refresh after the change was made to display current state
            RefreshDataGrid();

            //Cancel the search after the change was made
            UntickSearchOptions();
        }

        //Regular Search CheckBox Checked Event
        private void chkSearch_Checked(object sender, RoutedEventArgs e)
        {
            //Show the search options
            ShowAndHideSearchFields(eShowOrHide.Show);

            //Cancel Multisearch option
            chkMultiSearch.IsChecked = false;

            //Lower the flag of multiple search
            IsMultiSearch = false;
        }

        //Multiple Search CheckBox Checked Event
        private void chkMultiSearch_Checked(object sender, RoutedEventArgs e)
        {
            //Show the search options
            ShowAndHideSearchFields(eShowOrHide.Show);

            //Cancel regular search option
            chkSearch.IsChecked = false;

            //Hide relative controls which not valid for the multiple search
            GuiChanges.Hide(lblAuthor, txtAuthor, lblIssue, txtIssue);

            //Raise the flag of multiple search
            IsMultiSearch = true;
        }

        //Showing or Hiding the search controls and fields depending on received Enum
        private void ShowAndHideSearchFields(eShowOrHide showHide)
        {
            //Creating the Array of elements for simple handling of GUI,
            //instead of passing to GuiChanges class Methods each one of the controls,
            //pass only one array variable. Simple and Effective
            UIElement[] searchControls = { lblName, txtName, lblAuthor, txtAuthor,
                lblIssue, txtIssue, lblBaseCategory, cmbBaseCategory,
                lblInnerCategory, cmbInnerCategory };

            //Switch depending on enum
            switch (showHide)
            {
                //Show the search controls
                case eShowOrHide.Show:

                    //Show the elements from the created array
                    GuiChanges.Show(searchControls);

                    //Fill the ComboBox of Base category 
                    GuiChanges.FillComboWithBaseCategory(cmbBaseCategory);

                    //If no selection of Base Category made
                    if (cmbBaseCategory.SelectedItem == null)
                    {
                        //Hide the inner Category ComboBox
                        GuiChanges.Hide(lblInnerCategory, cmbInnerCategory);
                    }

                    break;

                //Hide the search controls
                case eShowOrHide.Hide:

                    //Hide the elements from the created array
                    GuiChanges.Hide(searchControls);

                    //Search for the text fields in elements array
                    foreach (UIElement item in searchControls)
                    {
                        //If the Control is Text Box
                        if (item is TextBox)
                        {
                            //Clear the contents of the Text Box, so
                            //this way there's no interfear with feauture search options
                            //when the fields will be enabled and visible again
                            ((TextBox)item).Text = string.Empty;
                        }
                    }

                    //Clean the Combo Boxes of Base and Inner Categories
                    cmbBaseCategory.ItemsSource = cmbInnerCategory.ItemsSource = null;

                    break;
            }
        }

        //Regular and Multiply Check Boxes UNCHECK EVENT
        private void HideSearch(object sender, RoutedEventArgs e)
        {
            //Hide the search options
            ShowAndHideSearchFields(eShowOrHide.Hide);

            //Refresh DataGrid so THere's no relults from previous search (if any)
            RefreshDataGrid();

            //Still if one of the CheckBoxes is checked - show the search Options
            if (chkMultiSearch.IsChecked == true || chkSearch.IsChecked == true)
            {
                ShowAndHideSearchFields(eShowOrHide.Show);
            }
        }

        private void IssueSearch(object sender, TextChangedEventArgs e)
        {
            if (!Validity.PositiveInteger(txtIssue.Text))
            {
                txtIssue.Text = string.Empty;
            }
            else if (!IsMultiSearch)
            {
                dataLib.ItemsSource =
                    mainLibrary.FindJournal(j => j.IssueNumber == int.Parse(txtIssue.Text));
            }
            if (string.IsNullOrWhiteSpace(txtIssue.Text))
            {
                RefreshDataGrid();
            }
        }

        private void FillInner()
        {
            UIElement[] inner = { lblInnerCategory, cmbInnerCategory };
            if (cmbBaseCategory.SelectedItem != null)
            {
                GuiChanges.FillComboWithInnerCategory
                    (cmbInnerCategory, cmbBaseCategory.SelectedItem);
                GuiChanges.Show(inner);
            }
            else
            {
                GuiChanges.Hide(inner);
            }
        }

        private void Focused(object sender, RoutedEventArgs e)
        {
            CleanFields(sender);
        }

        private void CleanFields(object exceptThis)
        {
            if (!IsMultiSearch)
            {
                foreach (UIElement item in this.grdWindowGrid.Children)
                {
                    if (item != exceptThis && item is TextBox)
                    {
                        ((TextBox)item).Text = string.Empty;
                    }
                    else if (item is TextBox && item.IsFocused)
                    {
                        cmbBaseCategory.SelectedIndex = -1;
                        RefreshDataGrid();
                    }
                }

            }
        }

        private void AuthorSearch(object sender, TextChangedEventArgs e)
        {
            if (!IsMultiSearch)
            {
                dataLib.ItemsSource =
                    mainLibrary.FindBook(b => b.Author.ToLower().Contains(txtAuthor.Text.ToLower()));
            }
            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                RefreshDataGrid();
            }
        }

        private void NameSearch(object sender, TextChangedEventArgs e)
        {
            if (!IsMultiSearch)
            {
                dataLib.ItemsSource = mainLibrary.FindItemByName(txtName.Text);
            }
            else
            {
                MultipleSearch();
            }
        }

        private void SearchBase(object sender, SelectionChangedEventArgs e)
        {
            FillInner();
            if (!IsMultiSearch && cmbBaseCategory.SelectedItem != null)
            {
                dataLib.ItemsSource =
                    mainLibrary.FindByBaseCategory((eBaseCategory)cmbBaseCategory.SelectedItem);
            }
            else if (IsMultiSearch)
            {
                MultipleSearch();
            }
        }

        private void SearchInnerByBase(object sender, SelectionChangedEventArgs e)
        {
            var eBase = cmbBaseCategory.SelectedItem;
            var eInner = cmbInnerCategory.SelectedItem;
            if (eBase != null && eInner != null && !IsMultiSearch)
            {
                dataLib.ItemsSource =
                    mainLibrary.FindInnerByBaseCategory((eBaseCategory)eBase, (eInnerCategory)eInner);
            }
            else if (IsMultiSearch)
            {
                MultipleSearch();
            }
        }

        private void MultipleSearch()
        {
            eBaseCategory? eBase =
                cmbBaseCategory.SelectedValue is eBaseCategory ?
                (eBaseCategory?)cmbBaseCategory.SelectedValue : null;

            eInnerCategory? eInner =
                cmbInnerCategory.SelectedValue is eInnerCategory ?
                (eInnerCategory?)cmbInnerCategory.SelectedValue : null;

            dataLib.ItemsSource = mainLibrary.MultiSearch(eBase, eInner, txtName.Text);
        }

        private void btnQuantity_Click(object sender, RoutedEventArgs e)
        {
            AbstractItem item = (AbstractItem)dataLib.SelectedItem;
            GuiMsgs.Info($"The quantity of this {item.ItemType} is {mainLibrary.ItemQuantity(item)}");
        }

        private void UntickSearchOptions()
        {
            chkSearch.IsChecked = chkMultiSearch.IsChecked = false;
        }
    }
}
