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

        /// <summary>
        /// User Types Enum
        /// </summary>
        public enum eUserType
        {
            Administrator,
            Employee,
            Client
        }

        /// <summary>
        /// User Class
        /// </summary>
        /// <param name="name">User Name</param>
        /// <param name="password">Password</param>
        /// <param name="type">User Type</param>
        public User(string name, string password, eUserType type)
        {
            ID++;
            Name = name;
            Password = password;
            Type = type;
        }
    }

    /// <summary>
    /// Managers all the Users
    /// </summary>
    [Serializable]
    public class UsersManager
    {
        /// <summary>
        /// Users container
        /// </summary>
        public List<User> Users = new List<User>()
        {
            new User( "BigBoss", "1", User.eUserType.Administrator )
        };

        /// <summary>
        /// Current User
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Finds User by given parameters
        /// </summary>
        /// <param name="name">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>User found</returns>
        public User GetCurrentUser(string name, string password)
        {
            return Users.FirstOrDefault(u => u.Name == name && u.Password == password);
        }

        /// <summary>
        /// Checks if given User has correct 
        /// username and password relatively
        /// </summary>
        /// <param name="name">User Name</param>
        /// <param name="pass">Password</param>
        /// <returns>
        /// True if username and password is same,
        /// False if not
        /// </returns>
        public bool CheckUser(string name, string pass)
        {
            if (Users.Any(u => u.Name == name && u.Password == pass))
                return true;
            else
                return false;
        }
    }
}
