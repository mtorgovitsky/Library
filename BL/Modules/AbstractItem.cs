using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static BL.Categories;

namespace BookLib
{
    public abstract class AbstractItem
    {
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
            BaseCategory = baseCategory;

        }

        private DateTime _printDate;
        private string _name;
        private int _copyCount;
        private eInnerCategory _innerCategory;

        public eBaseCategory BaseCategory { get; set; }

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

        public int CopyCount
        {
            get
            {
                return _copyCount;
            }
            set
            {
                _copyCount = value;
            }
        }

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
    }
}