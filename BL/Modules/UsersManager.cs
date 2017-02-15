using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Modules
{
    /// <summary>
    /// Users of the Library
    /// </summary>
    [Serializable]
    public class User
    {
        public readonly int UserID;
        public eUserType UserType { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public enum eUserType
        {
            Nan,
            Admin,
            Employee,
            SuperAdmin,
            LibraryManager
        }
    }

    public static class UsersManager
    {
        public static List<User> Users = new List<User>()
        {
            new User { Name = "superuser", Password = "123", UserType = User.eUserType.SuperAdmin },
            new User { Name = "employee", Password = "123", UserType = User.eUserType.Employee },
            new User { Name = "manager", Password = "123", UserType = User.eUserType.LibraryManager },
            new User { Name = "admin", Password = "1", UserType = User.eUserType.Admin }
        };

        public static bool CheckUser(string name, string pass)
        {
            //var currUser = Users.First(u => u.Name == name && u.Password == pass);
            if (Users.Any(u => u.Name == name && u.Password == pass))
                return true;
            else
                return false;
        }
    }
}
