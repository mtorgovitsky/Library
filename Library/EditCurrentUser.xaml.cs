using BL.Modules;
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

namespace Library
{
    /// <summary>
    /// Interaction logic for EditCurrentUser.xaml
    /// </summary>
    public partial class EditCurrentUser : Window
    {
        //If the window was initiated for Editing existing User or
        //to add the new one
        public static bool EditMode { get; set; }

        //User instance to deal with
        public static User UserToAddOrEdit { get; set; }

        //Ctor for our window
        public EditCurrentUser()
        {
            InitializeComponent();
            if (EditMode)
            {
                InitAllFields("Edit User", "Save User");
            }
            else
            {
                InitAllFields("New User", "Add New User");
            }
        }

        //Init all the controls of the window
        private void InitAllFields(string title, string button)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(User.eUserType));
            Title = title;
            btnSaveEdit.Content = button;
            if (EditMode) //if updating existing user
            {
                txtName.Text = UserToAddOrEdit.Name;
                txtPassword.Text = UserToAddOrEdit.Password;
                cmbType.SelectedItem = UserToAddOrEdit.Type;
            }
        }

        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            /////////////////////////   VALIDATION OF THE USER INPUT   //////////////////
            if (!Validity.StringOK(txtName.Text))
            {
                GuiMsgs.Warning("Please Enter User Name");
            }
            else if (!Validity.StringOK(txtPassword.Text))
            {
                GuiMsgs.Warning("Please Enter Password");
            }
            else if (cmbType.SelectedIndex < 0)
            {
                GuiMsgs.Warning("Please select the User Type");
            }
            /////////////////////////   VALIDATION OF THE USER INPUT   //////////////////
            else //if all fields are OK
            {
                switch (EditMode)
                {
                    case true: //updating existing user
                        UserToAddOrEdit.Name = txtName.Text;
                        UserToAddOrEdit.Password = txtPassword.Text;
                        UserToAddOrEdit.Type = (User.eUserType)cmbType.SelectedItem;
                        this.Close();
                        break;

                    case false: //adding new user
                        UserToAddOrEdit = new User(txtName.Text, txtPassword.Text, (User.eUserType)cmbType.SelectedItem);
                        MainWindow.mainLibrary.LibraryUsers.Users.Add(UserToAddOrEdit);
                        this.Close();
                        break;
                }
            }
        }
    }
}
