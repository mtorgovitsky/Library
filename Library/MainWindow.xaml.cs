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
            ShowAndHideSearchFields();
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
            //EditItem.Item = mainLibrary.FindAbstractItem(ai => ai.ISBN == tmp.ISBN).FirstOrDefault();
            //EditItem.Item = tmp;
        }

        public int GridSelected()
        {
            UIElement[] controlsForDataGrid = { btnEdit, btnDetails, btnDelete, btnBorrow };
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
                    GuiChanges.Disable(btnDelete, btnUsers, btnEdit, btnAdd, btnBorrow);
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
            if (choosenItem.IsBorrowed)
            {
                choosenItem.IsBorrowed = false;
            }
            else
            {
                choosenItem.IsBorrowed = true;
            }
            RefreshDataGrid();
        }

        private void NameSearch(object sender, TextChangedEventArgs e)
        {
            if (Validity.StringOK(txtName.Text))
            {
                dataLib.ItemsSource = mainLibrary.FindByName(txtName.Text);
            }
        }

        private void chkSearch_Checked(object sender, RoutedEventArgs e)
        {
            ShowAndHideSearchFields();
            chkMultiSearch.IsChecked = false;
            IsMultiSearch = false;
        }

        private void chkMultiSearch_Checked(object sender, RoutedEventArgs e)
        {
            ShowAndHideSearchFields();
            chkSearch.IsChecked = false;
            IsMultiSearch = true;
        }

        private void ShowAndHideSearchFields()
        {
            UIElement[] searchControls = { lblName, txtName, lblAuthor, txtAuthor, lblIssue, txtIssue,
                lblBaseCategory, cmbBaseCategory, lblInnerCategory, cmbInnerCategory };
            if (chkSearch.IsChecked == false && chkMultiSearch.IsChecked == false)
            {
                GuiChanges.Hide(searchControls);
                cmbBaseCategory.ItemsSource = cmbInnerCategory.ItemsSource = null;
            }
            else
            {
                GuiChanges.Show(searchControls);
                if (cmbBaseCategory.SelectedItem == null)
                {
                    GuiChanges.Hide(lblInnerCategory, cmbInnerCategory); 
                }
                GuiChanges.FillBaseCategory(cmbBaseCategory);
            }

        }

        private void HideSearch(object sender, RoutedEventArgs e)
        {
            ShowAndHideSearchFields();
        }

        private void IssueSearch(object sender, TextChangedEventArgs e)
        {
            if (!Validity.PositiveInteger(txtIssue.Text))
            {
                txtIssue.Text = string.Empty;
            }
        }

        private void FillInner(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBaseCategory.SelectedItem != null)
            {
                GuiChanges.FillInnerCategory(cmbInnerCategory, cmbBaseCategory.SelectedItem);
                GuiChanges.Show(lblInnerCategory, cmbInnerCategory);

            }
        }
    }
}
