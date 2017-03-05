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

        internal static void Warning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        internal static void Info(string message)
        {
            MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        internal static bool AreYouSure(string message)
        {
            var result = MessageBox.Show(message, "Really?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
