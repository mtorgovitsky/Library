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
        public static ItemsCollection mainLibrary = new ItemsCollection();

        public bool IsMultiSearch { get; set; }

        public enum eShowOrHide
        {
            Show,
            Hide
        }

        public MainWindow()
        {
            InitializeComponent();
            var login = new LoginWindow();
            login.ShowDialog();
            InitMainWindow();
        }

        private void InitMainWindow()
        {
            RefreshDataGrid();
            ShowAndHideSearchFields(eShowOrHide.Hide);
            dataLib.IsReadOnly = true;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            mainLibrary.SaveData(mainLibrary);
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addItem = new AddNewItem();
            addItem.ShowDialog();
            RefreshDataGrid();
        }

        private void ClosingMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainLibrary.SaveData(mainLibrary);
        }

        public void RefreshDataGrid()
        {
            dataLib.ItemsSource = mainLibrary.Items;
            dataLib.Items.Refresh();
            GridSelected();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditItem.EditMode = true;
            EditItem.CurrentItem = (AbstractItem)dataLib.SelectedItem;
            var editW = new EditItem();
            editW.ShowDialog();
            RefreshDataGrid();
        }

        public int GridSelected()
        {
            UIElement[] controlsForDataGrid = { btnEdit, btnDetails, btnDelete, btnBorrow, btnQuantity };
            if (dataLib.SelectedIndex >= 0 && dataLib.SelectedIndex < mainLibrary.Items.Count)
            {
                GuiChanges.Enable(controlsForDataGrid);
                ButtonsAvailable();
                var tmpItem = (AbstractItem)dataLib.SelectedItem;
                switch (tmpItem.IsBorrowed)
                {
                    case true:
                        btnBorrow.Content = $"Return {tmpItem.ItemType}";
                        break;
                    case false:
                        btnBorrow.Content = $"Borrow {tmpItem.ItemType}";
                        break;
                }
                return dataLib.SelectedIndex;
            }
            else
            {
                GuiChanges.Disable(controlsForDataGrid);
                ButtonsAvailable();
                return dataLib.SelectedIndex;
            }
        }

        private void dataLib_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridSelected();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            EditItem.EditMode = false;
            EditItem.CurrentItem = (AbstractItem)dataLib.SelectedItem;
            var tmpW = new EditItem();
            tmpW.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GuiMsgs.AreYouSure(
                "Are You Positive that You want to delete this Item?" +
                "\n(There's no way You can undo this action!)"))
            {
                mainLibrary.Items.Remove((AbstractItem)dataLib.SelectedItem);
                RefreshDataGrid();
            }
        }

        private void ButtonsAvailable()
        {
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

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            var editUsers = new EditUsers();
            editUsers.ShowDialog();
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            AbstractItem choosenItem = (AbstractItem)dataLib.SelectedItem;
            choosenItem.IsBorrowed = !choosenItem.IsBorrowed;
            RefreshDataGrid();
            chkMultiSearch.IsChecked = chkSearch.IsChecked = false;
            ShowAndHideSearchFields(eShowOrHide.Hide);
        }

        private void chkSearch_Checked(object sender, RoutedEventArgs e)
        {
            ShowAndHideSearchFields(eShowOrHide.Show);
            chkMultiSearch.IsChecked = false;
            IsMultiSearch = false;
        }

        private void chkMultiSearch_Checked(object sender, RoutedEventArgs e)
        {
            ShowAndHideSearchFields(eShowOrHide.Show);
            chkSearch.IsChecked = false;
            GuiChanges.Hide(lblAuthor, txtAuthor, lblIssue, txtIssue);
            IsMultiSearch = true;
        }

        private void ShowAndHideSearchFields(eShowOrHide showHide)
        {
            UIElement[] searchControls = { lblName, txtName, lblAuthor, txtAuthor,
                lblIssue, txtIssue, lblBaseCategory, cmbBaseCategory,
                lblInnerCategory, cmbInnerCategory };
            switch (showHide)
            {
                case eShowOrHide.Show:
                    GuiChanges.Show(searchControls);
                    GuiChanges.FillComboWithBaseCategory(cmbBaseCategory);
                    if (cmbBaseCategory.SelectedItem == null)
                    {
                        GuiChanges.Hide(lblInnerCategory, cmbInnerCategory);
                    }
                    break;
                case eShowOrHide.Hide:
                    GuiChanges.Hide(searchControls);
                    foreach (UIElement item in searchControls)
                    {
                        if (item is TextBox)
                        {
                            ((TextBox)item).Text = string.Empty;
                        }
                    }
                    cmbBaseCategory.ItemsSource = cmbInnerCategory.ItemsSource = null;
                    break;
                default:
                    break;
            }
        }


        private void HideSearch(object sender, RoutedEventArgs e)
        {
            ShowAndHideSearchFields(eShowOrHide.Hide);
            RefreshDataGrid();
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
                    else if(item is TextBox && item.IsFocused)
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
    }
}
