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
        public readonly int ID;
        public string Name { get; set; }
        public string Password { get; set; }
        public eUserType Type { get; set; }

        public enum eUserType
        {
            Nan,
            Admin,
            Employee,
            SuperAdmin,
            LibraryManager
        }

        public User(string name, string password, eUserType type)
        {
            ID++;
            Name = name;
            Password = password;
            Type = type;
        }
    }

    public static class UsersManager
    {
        public static List<User> Users = new List<User>()
        {
            new User( "admin", "1", User.eUserType.Admin ),
            new User( "superuser", "123", User.eUserType.SuperAdmin ),
            new User( "employee", "123", User.eUserType.Employee ),
            new User( "manager", "123",  User.eUserType.LibraryManager ),
        };


        public static User GetCurrentUser(string name, string password)
        {
            return Users.FirstOrDefault(u => u.Name == name && u.Password == password);
        }

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
