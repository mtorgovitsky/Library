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
            BLLibraryData blld = new BLLibraryData();
            blld.Items.Add
                (new Book
                        ("Book of Treasures",
                        DateTime.Now.AddYears(-8),
                        eBaseCategory.Cooking,
                        eInnerCategory.Soups,
                        "Ann Geronulasoftred"));
            blld.Items.Add
                (new Journal
                        ("Some Journal",
                        DateTime.Now.AddYears(-1),
                        eBaseCategory.Kids,
                        eInnerCategory.Comics,
                        6));
        }
    }
}
