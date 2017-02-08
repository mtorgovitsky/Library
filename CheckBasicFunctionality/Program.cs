using BL;
using BL.Modules;
using BookLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Categories;

namespace CheckBasicFunctionality
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemsCollection itemsC = new ItemsCollection();
            itemsC.Items.Add
                (new Book
                        ("Book of Treasures",
                        DateTime.Now.AddYears(-8),
                        eBaseCategory.Cooking,
                        eInnerCategory.Soups,
                        "Ann Geronulasoftred"));
            itemsC.Items.Add
                (new Journal
                        ("Some Journal",
                        DateTime.Now.AddYears(-1),
                        eBaseCategory.Kids,
                        eInnerCategory.Comics,
                        6));

            itemsC.SuperAdmin = new User("Admin", "12345678910");
            itemsC.SuperAdmin.UserType = User.eUserType.SuperAdmin;

            itemsC.SaveData(itemsC);
            ItemsCollection getData = itemsC.GetBLData();
            if(itemsC.SuperAdmin == null)
            {
                Console.WriteLine("No Super User");
            }
        }
    }
}
