using BL;
using BL.Modules;
using BookLib;
using System;
using System.Collections.Generic;
using System.IO;
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
using static BL.Categories;

namespace Library
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    [Serializable]
    public partial class LoginWindow : Window
    {
        public static ItemsCollection mainLibrary = new ItemsCollection();
        public LoginWindow()
        {
            InitializeComponent();
            if (File.Exists("DBData.osl"))
                mainLibrary = mainLibrary.GetBLData();
            if (mainLibrary.SuperAdmin == null)
                MessageBox.Show("You Entering The Program for the first Time.\n Please Make first Login");
            btnLogin.IsEnabled = false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainLibrary.SaveData(mainLibrary);
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnLogin.IsEnabled = true;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == string.Empty)
                MessageBox.Show("Password is required");
            else
            {
                mainLibrary.SuperAdmin = new User(txtUserName.Text, txtPassword.Text);

                //CheckDataSaving();

                mainLibrary.SaveData(mainLibrary);
            }
        }

        private static ItemsCollection CheckDataSaving()
        {
            mainLibrary.Items.Add(new Book
                                ("Book of Treasures",
                                DateTime.Now.AddYears(-8),
                                eBaseCategory.Cooking,
                                eInnerCategory.Soups,
                                "Ann Geronulasoftred"));
            mainLibrary.Items.Add(new Journal
                                ("Some Journal",
                                DateTime.Now.AddYears(-1),
                                eBaseCategory.Kids,
                                eInnerCategory.Comics,
                                6));
            mainLibrary.SuperAdmin = new User("dfdfd", "sdsasf");
            mainLibrary.SaveData(mainLibrary);
            var tmp = mainLibrary.GetBLData();
            return tmp;
        }
    }
}
