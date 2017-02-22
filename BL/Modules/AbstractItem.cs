using BL;
using BL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static BL.Categories;

namespace BookLib
{
    [Serializable]
    public abstract class AbstractItem : IEqual
    {
        public string ItemType
        {
            get
            {
                //Return the string with the name of the object
                return this.GetType().Name;
                //string[] tmpStr = this.GetType().ToString().Split('.');
                //return tmpStr[tmpStr.Length - 1];
            }
        }

        public readonly Guid ISBN;

        public bool IsBorrowed { get; set; }

        public AbstractItem
            (
                string name,
                DateTime printDate,
                eBaseCategory baseCategory,
                eInnerCategory innerCategory
            //int copyCount = 1
            )
        {
            ISBN = Guid.NewGuid();
            if (name != null)
                _name = name;
            if (printDate != null)
                _printDate = printDate;
            //if (copyCount > 1)
            //    _copyCount = copyCount;

            /////////////   CHECK THE CATEGORIES FUNCTIONALITY  //////////////
            if (Categories.CategoriesDictionary.ContainsKey(baseCategory))
            {
                var catList = Categories.CategoriesDictionary.FirstOrDefault(p => p.Key == baseCategory);
                if (catList.Value.Contains(innerCategory))
                {
                    BaseCategory = baseCategory;
                    InnerCategory = innerCategory;
                }
            }
            /////////////   CHECK THE CATEGORIES FUNCTIONALITY  //////////////

            PrintDate = printDate;
        }

        private DateTime _printDate;
        private string _name;
        //private int _copyCount;
        public eBaseCategory BaseCategory { get; set; }
        public eInnerCategory _innerCategory;

        public eInnerCategory InnerCategory
        {
            get
            {
                return _innerCategory;
            }
            set
            {
                if (Categories.CategoriesDictionary[BaseCategory].Contains(value))
                    _innerCategory = value;
            }
        }

        //public int CopyCount
        //{
        //    get
        //    {
        //        return _copyCount;
        //    }
        //    set
        //    {
        //        _copyCount = value;
        //    }
        //}

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
            }
        }

        public DateTime PrintDate
        {
            get
            {
                return _printDate;
            }
            set
            {
                _printDate = new DateTime(value.Year, value.Month, value.Day);
            }
        }

        public virtual bool Equal(AbstractItem item)
        {
            if (Name == item.Name && BaseCategory == item.BaseCategory
                && InnerCategory == item.InnerCategory &&
                PrintDate == item.PrintDate)
                return true;
            else
                return false;
        }
    }
}