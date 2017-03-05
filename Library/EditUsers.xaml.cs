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
            dataUsers.IsReadOnly = true;
            dataUsers.ItemsSource = MainWindow.mainLibrary.LibraryUsers.Users;
            GuiChanges.Disable(btnDelete, btnEdit);
        }

        private void Refresh()
        {
            dataUsers.Items.Refresh();
        }

        private void HidePass(object sender, RoutedEventArgs e)
        {
            dataUsers.Columns[1].Visibility = Visibility.Hidden;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCurrentUser())
            {
                if (GuiMsgs.AreYouSure($"Are You Sure that\nYou want to delete\n This User?"))
                {
                    MainWindow.mainLibrary.LibraryUsers.Users.Remove((User)dataUsers.SelectedItem);
                    Refresh();
                }
            }
        }
        private bool IsCurrentUser()
        {
            if (MainWindow.mainLibrary.LibraryUsers.CurrentUser.Name == ((User)dataUsers.SelectedItem).Name)
            {
                GuiMsgs.Info("Deletion or Editing of Current user is\nFORBIDDEN!");
                return true;
            }
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

        private bool SelectedIndexIsValid()
        {
            if (dataUsers.SelectedIndex >= 0
                && dataUsers.SelectedIndex < Enum.GetNames(typeof(User.eUserType)).ToList().Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EnableCommands(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedIndexIsValid())
            {
                GuiChanges.Enable(btnEdit, btnDelete);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditCurrentUser.EditMode = false;
            var addUser = new EditCurrentUser();
            addUser.ShowDialog();
            Refresh();
        }

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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
