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
    /// Interaction logic for EditUsers.xaml
    /// </summary>
    public partial class EditUsers : Window
    {
        public EditUsers()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //Preventing the DataGrid from inplace editing
            dataUsers.IsReadOnly = true;
            
            //set the source for the DataGrid
            dataUsers.ItemsSource = MainWindow.mainLibrary.LibraryUsers.Users;
            
            //Disable buttons because no User haven't been choosen
            GuiChanges.Disable(btnDelete, btnEdit);
        }

        //Method for quick refresh action on DataGrid
        private void Refresh()
        {
            dataUsers.Items.Refresh();
        }

        //Hiding the password column in the DataGrid
        private void HidePass(object sender, RoutedEventArgs e)
        {
            dataUsers.Columns[1].Visibility = Visibility.Hidden;
        }

        //Deleting user
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCurrentUser()) // So current user ca't be deleted
            {
                //Warning dialog - last chance to regret and if user approves -
                //deleting the Item form the Library
                if (GuiMsgs.AreYouSure($"Are You Sure that\nYou want to delete the\n'{((User)dataUsers.SelectedItem).Name}'\nUser?"))
                {
                    //Deletion
                    MainWindow.mainLibrary.LibraryUsers.Users.Remove((User)dataUsers.SelectedItem);

                    //Refresh the Grid after the change was made to the Item collection
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Check if User from DataGrid is the current User
        /// </summary>
        /// <returns>If the User is current - true, opposite - false</returns>
        private bool IsCurrentUser()
        {
            //if the user is current - prevent deletion or editing
            if (MainWindow.mainLibrary.LibraryUsers.CurrentUser.Name == ((User)dataUsers.SelectedItem).Name)
            {
                GuiMsgs.Info("Deletion or Editing of Current user is\nFORBIDDEN!");
                return true;
            }
            
            //if the user is main user - also prevent deletion or editing
            if (((User)dataUsers.SelectedItem).Name == "BigBoss")
            {
                GuiMsgs.Info("Deletion or Editing of the 'Big Boss' is\nFORBIDDEN!\nThis is the Main User,\nSo It's Impossible to Delete him");
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Prevent exception on choosing Column name or under the last row in the DataGrid
        /// </summary>
        /// <returns>If the index is in valid range - true or false</returns>
        private bool SelectedIndexIsValid()
        {
            if (dataUsers.SelectedIndex >= 0
                && dataUsers.SelectedIndex < MainWindow.mainLibrary.LibraryUsers.Users.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Enable "edit" and "delete" buttons if the Selected Index is valid
        private void EnableCommands(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedIndexIsValid())
            {
                GuiChanges.Enable(btnEdit, btnDelete);
            }
        }

        //Pop up the add new user dialog
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditCurrentUser.EditMode = false;

            var addUser = new EditCurrentUser();
            addUser.ShowDialog();

            //Refresh after possible changes from user
            Refresh();
        }

        //Editing the users - using the same dialog window as for add new
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCurrentUser())
            {
                EditCurrentUser.EditMode = true;

                EditCurrentUser.UserToAddOrEdit = (User)dataUsers.SelectedItem;

                var editUser = new EditCurrentUser();
                editUser.ShowDialog();

                Refresh();
            }
        }

        //close the dialog
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
