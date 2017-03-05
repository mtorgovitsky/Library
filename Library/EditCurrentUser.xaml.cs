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
        public static bool EditMode { get; set; }
        public static User UserToAddOrEdit { get; set; }

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

        private void InitAllFields(string title, string button)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(User.eUserType));
            Title = title;
            btnSaveEdit.Content = button;
            if (EditMode)
            {
                txtName.Text = UserToAddOrEdit.Name;
                txtPassword.Text = UserToAddOrEdit.Password;
                cmbType.SelectedItem = UserToAddOrEdit.Type;
            }
        }

        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if(Validity.StringValid(txtName.Text))
            {

            }
        }
    }
}
