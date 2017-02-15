﻿using BL;
using BookLib;
using Data;
using Library.GUI;
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

        public MainWindow()
        {
            InitializeComponent();
            //mainLibrary = mainLibrary.GetBLData();
            var login = new LoginWindow();
            login.ShowDialog();

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            mainLibrary.SaveData(mainLibrary);
            this.Close();
        }

        //private static ItemsCollection CheckDataSaving()
        //{
        //    mainLibrary.Items.Add(new Book
        //                        ("Book of Treasures",
        //                        DateTime.Now.AddYears(-8),
        //                        eBaseCategory.Cooking,
        //                        eInnerCategory.Soups,
        //                        "Ann Geronulasoftred"));
        //    mainLibrary.Items.Add(new Journal
        //                        ("Some Journal",
        //                        DateTime.Now.AddYears(-1),
        //                        eBaseCategory.Kids,
        //                        eInnerCategory.Comics,
        //                        6));
        //    mainLibrary.SaveData(mainLibrary);
        //    var tmp = mainLibrary.GetBLData();
        //    return tmp;
        //}

        private void btnCheckData_Click(object sender, RoutedEventArgs e)
        {
            var ic = new ItemsCollection();
            if (ic != null)
            {
                dataLib.ItemsSource = ic.GetBLData().Items;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addItem = new AddNewItem();
            addItem.ShowDialog();
        }

        private void SaveData(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainLibrary.SaveData(mainLibrary);
        }
    }
}
