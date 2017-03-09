using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.Modules
{
    /// <summary>
    /// Static Class for EASY Message Box manipulations
    /// </summary>
    public static class GuiMsgs
    {
        /// <summary>
        /// First Login message
        /// </summary>
        public static void FirstLogin()
        {
            MessageBox.Show
                ("Running The Library Program" +
                "\nFOR THE FIRST TIME" +
                "\nPlease obtain user name and password" +
                "\nfrom Your IT team (System Manager)." +
                "\n\nIf there's No One Arround, Just Enter \"BigBoss\"" +
                "\nfor the User Name and \"1\" for the Password" +
                "\n\nAnd may the power be with You!" +
                "\nI mean electricity ofcourse :)",
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
