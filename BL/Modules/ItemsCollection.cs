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
        /// <summary>
        /// Base Collection of the abstract Items for holding
        /// the Items of the Library - Books and Journals
        /// </summary>
        public List<AbstractItem> Items { get; set; }

        /// <summary>
        /// Collections of Users for the Library
        /// </summary>
        public UsersManager LibraryUsers;

        /// <summary>
        /// Ctor  - instantiates two collections:
        /// "Items" and "Users"
        /// </summary>
        public ItemsCollection()
        {
            Items = new List<AbstractItem>();
            LibraryUsers = new UsersManager();
        }

        /// <summary>
        /// Saves entire Library to the local file by Serialization
        /// </summary>
        /// <param name="data">Data to save</param>
        public void SaveData(ItemsCollection data)
        {
            DBData.Serialize(data);
        }

        /// <summary>
        /// Returns the data from local file by desirialization
        /// </summary>
        /// <returns>Desirializated data</returns>
        public ItemsCollection GetBLData()
        {
            var data = DBData.DeSerialize<ItemsCollection>();
            return data;
        }


        /// <summary>
        /// Checks if the Item is not Null and
        /// adds the Item to collection
        /// </summary>
        /// <param name="abstItem"></param>
        public void Add(AbstractItem abstItem)
        {
            if (abstItem != null)
                Items.Add(abstItem);
        }

        /// <summary>
        /// Predicate function - for future use
        /// currently not referenced in the project
        /// </summary>
        /// <param name="p">Predicate</param>
        /// <returns>List of the AbstractItem's</returns>
        public List<AbstractItem> ItemsToList(Func<AbstractItem, bool> p)
        {
            return Items.Where(p).ToList();
        }

        /// <summary>
        /// Finds Item by Name
        /// </summary>
        /// <param name="name">Name to compare</param>
        /// <returns>List of the AbstractItem's found</returns>
        public List<AbstractItem> FindItemByName(string name)
        {
            return Items.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseCategory"></param>
        /// <returns></returns>
        public List<AbstractItem> FindByBaseCategory(eBaseCategory baseCategory)
        {
            return Items.Where(i => i.BaseCategory == baseCategory).ToList();
        }

        public List<AbstractItem> FindByInnerCategory(eInnerCategory innerCategory)
        {
            return Items.Where(i => i.InnerCategory == innerCategory).ToList();
        }

        public List<AbstractItem> FindInnerByBaseCategory(eBaseCategory baseCategory,
                                    eInnerCategory innerCategory)
        {
            return Items.Where(i => i.BaseCategory == baseCategory && i.InnerCategory == innerCategory).ToList();
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
                .Where(i => i.Name == name.ToLower())
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
            return Items
                 .OfType<Book>()
                 .Where(i => i.Author == author.ToLower())
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

        //public bool Lending(AbstractItem item)
        //{
        //    if (item.IsBorrowed)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        item.IsBorrowed = !item.IsBorrowed;
        //        return true;
        //    }
        //}

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

        public List<AbstractItem> MultiSearch(
            eBaseCategory? eBase, eInnerCategory? eInner, string name)
        {
            bool baseCheck = eBase.HasValue ? true : false;
            bool innerCheck = eInner.HasValue ? true : false;
            return FindAbstractItem(ai =>
                ai.Name.ToLower().Contains(name.ToLower())
                && (baseCheck ? (ai.BaseCategory == eBase) : true)
                && (innerCheck ? (ai.InnerCategory == eInner) : true)).ToList();
        }
    }
}
