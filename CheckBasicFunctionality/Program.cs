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
            SaveData();
            var tmp = GetData();
            //if(itemsC.SuperAdmin == null)
            //{
            //    Console.WriteLine("No Super User");
            //}
        }

        private static void CheckDataSaving()
        {
            throw new NotImplementedException();
        }

        private static void SaveData()
        {
            ItemsCollection ic = new ItemsCollection();
            //itemsC.SuperAdmin = new User("Admin", "12345678910");
            //itemsC.SuperAdmin.UserType = User.eUserType.SuperAdmin;
            ic.Items.Add(new Book
                ("Book of Treasures",
                DateTime.Now.AddYears(-8),
                eBaseCategory.Cooking,
                eInnerCategory.Soups,
                "Ann Geronulasoftred"));
            ic.Items.Add(new Journal
                ("Some Journal",
                DateTime.Now.AddYears(-1),
                eBaseCategory.Kids,
                eInnerCategory.Comics,
                6));

            ic.SaveData(ic);
        }

        private static ItemsCollection GetData()
        {
            ItemsCollection retIc = new ItemsCollection();
            return retIc.GetBLData();
        }
    }
}
