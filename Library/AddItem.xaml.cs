﻿using BL;
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
        public ItemType CurrentItem { get; set; }
        public enum ItemType
        {
            Default,
            Book,
            Journal
        }

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
            cmbInnerCat.SelectedIndex = 0;
        }

        private void rdChecked(object sender, RoutedEventArgs e)
        {
            var rdValue = sender as RadioButton;
            switch (rdValue.Name)
            {
                case "rdBook":
                    CurrentItem = ItemType.Book;
                    GuiChanges.Hide(lblIssue, txtIssue);
                    GuiChanges.Show(lblAuthor, txtAuthor);
                    break;
                case "rdJournal":
                    CurrentItem = ItemType.Journal;
                    GuiChanges.Hide(lblAuthor, txtAuthor);
                    GuiChanges.Show(lblIssue, txtIssue);
                    break;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            /////////////////////////   VALIDATION OF THE USER INPUT   //////////////////
            if (CurrentItem == ItemType.Default)
            {
                GuiMsgs.Warning
                   ("Please Choose the Item Type:" +
                   "\nBook or Journal!");
            }
            //if Base category is choosen
            else if (cmbBaseCat.SelectedIndex < 0)
            {
                GuiMsgs.Warning("Please choose the Base Category!");
            }
            else if (txtName.Text == string.Empty)
            {
                GuiMsgs.Warning("Please Enter The Name of The Item!");
            }
            else if (dtPick.SelectedDate == null)
            {
                GuiMsgs.Warning("Please Select a Publishing Date!");
            }
            else
            {
                switch (CurrentItem)
                {
                    case ItemType.Book:
                        if (txtAuthor.Text == string.Empty)
                        {
                            GuiMsgs.Warning("Please Enter The Author Name");
                        }
                        else
                        {
                            CreateItem();
                        }
                        break;
                    case ItemType.Journal:
                        if (txtIssue.Text == string.Empty)
                        {
                            GuiMsgs.Warning("Please Enter The Issue Number");
                        }
                        else if (!Validity.PositiveInteger(txtIssue.Text))
                        {
                            GuiMsgs.Warning("Please Enter the valid Issue Number");
                        }
                        else
                        {
                            CreateItem();
                        }
                        break;
                }
            }
            /////////////////////////   VALIDATION OF THE USER INPUT   //////////////////
        }

        public void CreateItem()
        {
            switch (CurrentItem)
            {
                case ItemType.Book:
                    MainWindow.mainLibrary.Items.Add(new Book(
                        txtName.Text,
                        dtPick.SelectedDate.GetValueOrDefault(),
                        (Categories.eBaseCategory)cmbBaseCat.SelectedItem,
                        (Categories.eInnerCategory)cmbInnerCat.SelectedItem,
                        txtAuthor.Text));
                    break;
                case ItemType.Journal:
                    MainWindow.mainLibrary.Items.Add(new Journal
                        (txtName.Text,
                        dtPick.SelectedDate.GetValueOrDefault(),
                        (Categories.eBaseCategory)cmbBaseCat.SelectedItem,
                        (Categories.eInnerCategory)cmbInnerCat.SelectedItem,
                        int.Parse(txtIssue.Text)));
                    break;
            }
        }
    }
}
