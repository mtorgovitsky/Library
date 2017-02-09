using BL.Modules;
using BookLib;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Categories;

namespace BL
{
    /// <summary>
    /// Class Responsable for managing Collections of Library Items
    /// </summary>
    [Serializable]
    public class ItemsCollection
    {
        public List<AbstractItem> Items { get; set; }
        public User SuperAdmin { get; set; }
        public List<User> Users { get; set; }

        public ItemsCollection()
        {
            Items = new List<AbstractItem>();
            Users = new List<User>();
            //SaveData(this);
        }



        public void SaveData(ItemsCollection data)
        {
            DBData.Serialize(data);
        }

        public ItemsCollection GetBLData()
        {
            var data = DBData.DeSerialize<ItemsCollection>();
            return data;
        }



        public void Add(AbstractItem abstItem)
        {
            if (abstItem != null)
                Items.Add(abstItem);
        }

        public List<AbstractItem> ItemsToList(Func<AbstractItem, bool> p)
        {
            return Items.Where(p).ToList();
        }

        //public int HowManyItems(AbstractItem item)
        //{
        //    if (item.GetType() == typeof(Journal))
        //    {
        //        var journals = _items.OfType<Journal>();
        //        int result =

        //    }
        //    if (item.GetType() == typeof(Book))
        //    {

        //    }
        //}

        public List<AbstractItem> FindByName(string name)
        {
            name = name.ToLower();
            return Items.Where(i => i.Name == name).ToList();
            //return ItemsToList(p => p.Name == name).ToList();
        }

        public List<AbstractItem> FindByBaseCategory(eBaseCategory baseCategory)
        {
            return Items.Where(i => i.BaseCategory == baseCategory).ToList();
            //return ItemsToList.
        }

        public List<AbstractItem> FindByInnerCategory(eInnerCategory innerCategory)
        {
            return Items.Where(i => i.InnerCategory == innerCategory).ToList();
        }

        //public List<AbstractItem> FindByMinCopies(int minCopies)
        //{
        //    return _items.Where(i => i.CopyCount >= minCopies).ToList();
        //}

        public List<AbstractItem> FindByPrintDate(DateTime printDate)
        {
            return Items.
                Where(i => i.PrintDate == printDate)
                .OrderBy(i => i.Name)
                .ToList();
        }

        public List<Journal> FindJournalByNameIssueDate(string name, int issueNumber, DateTime printDate)
        {
            name = name.ToLower();

            return Items
                .OfType<Journal>()
                .Where(i => i.Name == name)
                .Where(i => i.IssueNumber == issueNumber)
                .Where(i => i.PrintDate == printDate)
                .ToList();

            //return _items
            //    .Cast<Journal>() //------------------>Possible exception!!!
            //    .Where(i => i.Name == name)
            //    .Where(i => i.IssueNumber == issueNumber)
            //    .Where(i => i.PrintDate == printedDate)
            //    .ToList();
        }

        public List<Book> FindBookByAuthor(string author)
        {
            author = author.ToLower();

            return Items
                 .OfType<Book>()
                 .Where(i => i.Author == author)
                 .ToList();
        }


        public List<Book> FindBook(Func<Book, bool> p)
        {
            var lst = Items.OfType<Book>();
            return lst.Where(p).ToList();
        }

        public List<Journal> FindJournal(Func<Journal, bool> p)
        {
            var lst = Items.OfType<Journal>();
            return lst.Where(p).ToList();
        }

        public List<AbstractItem> FindAbstractItem(Func<AbstractItem, bool> p)
        {
            return Items.Where(p).ToList();
        }

        public bool Lending(AbstractItem item)
        {
            if (item.IsBorrowed)
            {
                return false;
            }
            else
            {
                item.IsBorrowed = !item.IsBorrowed;
                return true;
            }
        }

        public int ItemQuantity(AbstractItem item)
        {
            int result = 0;
            if (item is Book)
            {
                var lst = FindBook(i => i.Equal(item));
                result = lst.Count;
            }
            else
            {
                var lst = FindJournal(i => i.Equal(item));
                result = lst.Count;
            }
            return result;
        }
    }
}
