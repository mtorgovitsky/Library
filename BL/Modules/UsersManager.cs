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
        /// <summary>
        /// ID - static for incremention by one
        /// for each new user
        /// </summary>
        public static int ID;
        public string Name { get; set; }
        public string Password { get; set; }
        public eUserType Type { get; set; }

        public enum eUserType
        {
            Administrator,
            Employee,
            Client
        }

        public User(string name, string password, eUserType type)
        {
            ID++;
            Name = name;
            Password = password;
            Type = type;
        }
    }

    [Serializable]
    public class UsersManager
    {
        public List<User> Users = new List<User>()
        {
            new User( "BigBoss", "1", User.eUserType.Administrator )
        };

        public User CurrentUser { get; set; }

        public User GetCurrentUser(string name, string password)
        {
            return Users.FirstOrDefault(u => u.Name == name && u.Password == password);
        }

        public bool CheckUser(string name, string pass)
        {
            if (Users.Any(u => u.Name == name && u.Password == pass))
                return true;
            else
                return false;
        }
    }
}
