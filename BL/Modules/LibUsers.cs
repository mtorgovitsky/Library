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
    public class LibUsers
    {
        public readonly int UserID;
        public eUserType UserType { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<LibUsers> Users { get; set; }

        public LibUsers(string name, string password)
        {
            Name = name;
            Password = password;
            UserID += 1;
        }

        public enum eUserType
        {
            Nan,
            Admin,
            Employee,
            SuperAdmin,
            LibraryManager
        }

        public LibUsers GetCurrentUser(string name, string password)
        {
            name = name.ToLower();
            return Users
                .Where(u => u.Name == name && u.Password == password) 
                .FirstOrDefault();
                
        }
    }
}
