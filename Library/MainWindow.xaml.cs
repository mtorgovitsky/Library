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

        //Event for Issue Search TextBox,
        //Search for the Journal by It's Issue number
        private void IssueSearch(object sender, TextChangedEventArgs e)
        {
            //if the the User inputs is not Positive integer - 
            //don't search, instead clear the TextBox contents
            if (!Validity.PositiveInteger(txtIssue.Text))
            {
                txtIssue.Text = string.Empty;
            }
            //If we in the regular search mode (not multiply)
            else if (!IsMultiSearch)
            {
                dataLib.ItemsSource =
                    mainLibrary.FindJournal(j => j.IssueNumber == int.Parse(txtIssue.Text));
            }
            //if the user deletes the text from issue search box,
            //Show all the item in the library
            if (string.IsNullOrWhiteSpace(txtIssue.Text))
            {
                RefreshDataGrid();
            }
        }


        //Fills the Inner Category ComboBox or hides it - 
        //depending on user input
        private void FillInner()
        {
            //Creating an array for easy hiding and showing
            //the Inner Category controls
            UIElement[] inner = { lblInnerCategory, cmbInnerCategory };

            //If there's any choice from the User on the Base Category ComboBox
            if (cmbBaseCategory.SelectedItem != null)
            {
                //Then fill the Inner Category ComboBox
                GuiChanges.FillComboWithInnerCategory
                    (cmbInnerCategory, cmbBaseCategory.SelectedItem);

                //And show the Inner ComboBox to the user
                GuiChanges.Show(inner);
            }
            //If there's no choice from the User on Base Category ComboBox
            else
            {
                //if so - hide the Inner Category ComboBox
                GuiChanges.Hide(inner);
            }
        }

        //Event, which comming from all the search controls
        private void Focused(object sender, RoutedEventArgs e)
        {
            //Send to CleanFields method refernce
            //to the object, which triggers the event
            CleanFields(sender);
        }

        //Receives the reference to the UIElement control,
        //who's triggered the event, and cleans all the search
        //fields EXCEPT the field which has triggered the event,
        //if we are currently in Regular Search Mode
        private void CleanFields(object exceptThis)
        {
            //if we are in Regular Search Mode
            if (!IsMultiSearch)
            {
                //For all the elements in the window
                foreach (UIElement item in this.grdWindowGrid.Children)
                {
                    //if the element is NOT the element which triggered the event
                    //and the element is of Type TextBox
                    if (item != exceptThis && item is TextBox)
                    {
                        //Clean the contents of the control
                        ((TextBox)item).Text = string.Empty;
                    }
                    //If the element is TextBox and it is In Focus
                    else if (item is TextBox && item.IsFocused)
                    {
                        //Set the Base Category Search selection to -1
                        //because the user allowed to search only
                        //by ONE CRITERIA while we in Regular search mode
                        cmbBaseCategory.SelectedIndex = -1;

                        //Refresh DataGrid to nullify search by Base Category
                        RefreshDataGrid();
                    }
                }

            }
        }

        //Search the Library for the Books by Author Name
        //and display the results in the DataGrid
        private void AuthorSearch(object sender, TextChangedEventArgs e)
        {
            //If we are in Regular Search Mode
            if (!IsMultiSearch)
            {
                //Search for the books by Author Name
                //and display the results in the DataGrid
                dataLib.ItemsSource =
                    mainLibrary.FindBook(b => b.Author.ToLower().Contains(txtAuthor.Text.ToLower()));
            }
            //If User inputs white spaces in the Search by Author TextBox -
            //Show all the Items in the Library
            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                RefreshDataGrid();
            }
        }


        //Search the Library for Books and Journals by Item Name
        //and display the results in the DataGrid
        private void NameSearch(object sender, TextChangedEventArgs e)
        {
            //If we are in Regular Search Mode
            if (!IsMultiSearch)
            {
                //Search for the books by Item Name
                //and display the results in the DataGrid
                dataLib.ItemsSource = mainLibrary.FindItemByName(txtName.Text);
            }
            //If we are in the Miltiply Serarch Mode
            else
            {
                //Make Multiply Search by numerous parameters
                //including search by Item Name
                MultipleSearch();
            }
        }

        //Event Triggered by Base Category ComboBox on "SelectionChange"
        //Search the Library by Base Category
        //and display the results in the DataGrid
        private void SearchBase(object sender, SelectionChangedEventArgs e)
        {
            //Fill the Inner Category Control
            //By Selected Base Category
            FillInner();

            //If we are in Regular Search mode and user
            //chooses one of the Base Category options
            if (!IsMultiSearch && cmbBaseCategory.SelectedItem != null)
            {
                //Search the Library by Base Category
                //and display the results in the DataGrid
                dataLib.ItemsSource =
                    mainLibrary.FindByBaseCategory((eBaseCategory)cmbBaseCategory.SelectedItem);
            }
            else if (IsMultiSearch)
            {
                //Make Multiply Search by numerous parameters
                //including search by Base Category
                MultipleSearch();
            }
        }

        //Event Triggered by Inner Category ComboBox on "SelectionChange"
        //REFINES the Base Category Search by Inner Category,
        //in othe words searches For the Items form base Category by Inner Category
        private void SearchInnerByBase(object sender, SelectionChangedEventArgs e)
        {
            //Base Category choosed by the User
            var eBase = cmbBaseCategory.SelectedItem;

            //Inner Category Choosed by the User
            var eInner = cmbInnerCategory.SelectedItem;

            //If we are in the Regular Search Mode
            //and there's valid data in the Base and Inner 
            //Categories ComboBoxes
            if (eBase != null && eInner != null && !IsMultiSearch)
            {
                //Refine the search from Base Category by Inner Category
                //and display the results in the DataGrid
                dataLib.ItemsSource =
                    mainLibrary.FindInnerByBaseCategory((eBaseCategory)eBase, (eInnerCategory)eInner);
            }
            else if (IsMultiSearch)
            {
                //Make Multiply Search by numerous parameters
                //including search form Base Category,
                //which was refined by Inner Category
                MultipleSearch();
            }
        }

        //Searches the Librari for Items by numerous parameters
        //and display the results in the DataGrid
        private void MultipleSearch()
        {
            //Base Category parameter (nullable)
            //the default value is true for using it in predicate
            //if User haven't choose the Base Category
            eBaseCategory? eBase =
                cmbBaseCategory.SelectedValue is eBaseCategory ?
                (eBaseCategory?)cmbBaseCategory.SelectedValue : null;

            //Inner Category parameter (nullable)
            //the default value is true for using it in predicate
            //if User haven't choose the Inner Category
            eInnerCategory? eInner =
                cmbInnerCategory.SelectedValue is eInnerCategory ?
                (eInnerCategory?)cmbInnerCategory.SelectedValue : null;

            //Make Multiply Search by numerous parameters
            //and display the results in the DataGrid
            dataLib.ItemsSource = mainLibrary.MultiSearch(eBase, eInner, txtName.Text);
        }

        //Search for Copies of the Item by relevant parameters
        //Excluding the ISBN, which is unique for each Item
        private void btnQuantity_Click(object sender, RoutedEventArgs e)
        {
            //Get the Item to search for from the DataGrid
            AbstractItem item = (AbstractItem)dataLib.SelectedItem;

            //Show the result of the Message Box
            GuiMsgs.Info($"The quantity of this {item.ItemType} is {mainLibrary.ItemQuantity(item)}");
        }

        //Utility method which unticks the 
        private void UntickSearchOptions()
        {
            chkSearch.IsChecked = chkMultiSearch.IsChecked = false;
        }
    }
}
