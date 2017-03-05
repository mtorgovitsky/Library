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
        public static User CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //mainLibrary = mainLibrary.GetBLData();
            var login = new LoginWindow();
            login.ShowDialog();
            RefreshDataGrid();
            dataLib.IsReadOnly = true;
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
            EditItem.Item = (AbstractItem)dataLib.SelectedItem;
            //EditItem.Item = mainLibrary.FindAbstractItem(ai => ai.ISBN == tmp.ISBN).FirstOrDefault();
            //EditItem.Item = tmp;
            var editW = new EditItem();
            editW.ShowDialog();
        }

        public int GridSelected()
        {
            if (dataLib.SelectedIndex >= 0 && dataLib.SelectedIndex < mainLibrary.Items.Count)
            {
                GuiChanges.Enable(btnEdit, btnDetails);
                return dataLib.SelectedIndex;
            }
            else
            {
                GuiChanges.Disable(btnEdit, btnDetails);
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
            EditItem.Item = (AbstractItem)dataLib.SelectedItem;
            var tmpW = new EditItem();
            tmpW.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
