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
        //Flag for showing or not the main window
        public static bool ShowMain = false;

        //Catching OnClosing Event to prevent Closing of the login dialog
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!ShowMain)
                e.Cancel = true;
        }

        //Ctor of Login Dialog
        public LoginWindow()
        {
            InitializeComponent();
            //Try open the Data File and read the Info from it.
            try
            {
                //if there's file at all
                if (File.Exists(DBData.FilePath))
                    MainWindow.mainLibrary = MainWindow.mainLibrary.GetBLData();
                //if it's the first initiation of the program - because there's no data file
                else
                {
                    GuiMsgs.FirstLogin();
                }

            }
            //catching the possible Exception and showing to the user with warning sign
            catch (Exception ex)
            {
                GuiMsgs.Warning(ex.Message);
            }
            //Disable the login button until the user enters some characters
            //in both fields
            GuiChanges.Disable(btnLogin);
            txtUserName.HorizontalContentAlignment =
                pswPassword.HorizontalContentAlignment = HorizontalAlignment.Center;
            pswPassword.PasswordChar = '*';
            //Focusing the keyboard into username field
            txtUserName.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Check if the input from the user is correct
            //if not - show message and clear the input fields
            if (!MainWindow.mainLibrary.LibraryUsers.CheckUser(txtUserName.Text, pswPassword.Password))
            {
                GuiMsgs.LoginFailed();
                txtUserName.Text = pswPassword.Password = string.Empty;
                txtUserName.Focus();
            }
            //if the input is correct - insert the user data
            //to the current user variable and show the main window
            else
            {
                MainWindow.mainLibrary.LibraryUsers.CurrentUser = 
                    MainWindow.mainLibrary.LibraryUsers.GetCurrentUser(txtUserName.Text, pswPassword.Password);
                ShowMain = true;
                this.Close();
            }
        }

        //checks the validity of input from the user and enables
        //or disables the login button relatively
        private void CanLogin()
        {
            if (!Validity.StringOK(pswPassword.Password) || !Validity.StringOK(txtUserName.Text))
                GuiChanges.Disable(btnLogin);
            else
                GuiChanges.Enable(btnLogin);
        }

        //if there's change with the password field - check
        //if can enable the login button
        private void pswPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CanLogin();
        }

        //if there's change with the username field - check
        //if can enable the login button
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CanLogin();
        }
    }
}
