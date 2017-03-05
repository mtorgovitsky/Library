using BL;
using BL.Modules;
using BookLib;
using Library.Modules;
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
using System.ComponentModel;
using Data;

namespace Library
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    [Serializable]
    public partial class LoginWindow : Window
    {
        public static bool ShowMain = false;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!ShowMain)
                e.Cancel = true;
        }

        //public static ItemsCollection mainLibrary = new ItemsCollection();
        public LoginWindow()
        {
            InitializeComponent();
            if (File.Exists(DBData.FilePath))
                MainWindow.mainLibrary = MainWindow.mainLibrary.GetBLData();
            else
            {
                GuiMsgs.FirstLogin();
            }
            btnLogin.IsEnabled = false;
            txtUserName.HorizontalContentAlignment =
                pswPassword.HorizontalContentAlignment = HorizontalAlignment.Center;
            pswPassword.PasswordChar = '*';
            txtUserName.Focus();
        }

        //private void btnExit_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //    mainLibrary.SaveData(mainLibrary);
        //}

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.mainLibrary.LibraryUsers.CheckUser(txtUserName.Text, pswPassword.Password))
            {
                GuiMsgs.LoginFailed();
                txtUserName.Text = pswPassword.Password = string.Empty;
                txtUserName.Focus();
            }
            else
            {
                MainWindow.mainLibrary.LibraryUsers.CurrentUser = MainWindow.mainLibrary.LibraryUsers.GetCurrentUser(txtUserName.Text, pswPassword.Password);
                ShowMain = true;
                this.Close();
            }
            //CheckDataSaving();
            //mainLibrary.SaveData(mainLibrary);
        }

        //private static ItemsCollection CheckDataSaving()
        //{
        //    MainWindow.mainLibrary.Items.Add(new Book
        //                        ("Book of Treasures",
        //                        DateTime.Now.AddYears(-8),
        //                        eBaseCategory.Cooking,
        //                        eInnerCategory.Soups,
        //                        "Ann Geronulasoftred"));
        //    MainWindow.mainLibrary.Items.Add(new Journal
        //                        ("Some Journal",
        //                        DateTime.Now.AddYears(-1),
        //                        eBaseCategory.Kids,
        //                        eInnerCategory.Comics,
        //                        6));
        //    MainWindow.mainLibrary.SaveData(mainLibrary);
        //    var tmp = MainWindow.mainLibrary.GetBLData();
        //    return tmp;
        //}

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            CanLogin();
        }

        private void CanLogin()
        {
            if (!Validity.StringOK(pswPassword.Password) || !Validity.StringOK(txtUserName.Text))
                btnLogin.IsEnabled = false;
            else
                btnLogin.IsEnabled = true;
        }

        private void pswPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CanLogin();
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CanLogin();
        }
    }
}
