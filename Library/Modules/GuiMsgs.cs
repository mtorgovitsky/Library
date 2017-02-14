using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.Modules
{
    public static class GuiMsgs
    {
        public static void FirstLogin()
        {
            MessageBox.Show
                ("Running The Library Program" +
                "\nFOR THE FIRST TIME" +
                "\nPlease obtain user name and password" +
                "\nfrom Your IT team (System Manager)",
                "First Login",
                MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
        }
        public static void LoginFailed()
        {
            MessageBox.Show
                ("The user name or password is incorrect" +
                "\nPlease try again...",
                "Login failed!",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
