using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BL;

namespace BookLib
{
    [Serializable]
    public class Book : AbstractItem
    {
        public Book(
            string name,
            DateTime printDate,
            Categories.eBaseCategory baseCategory,
            Categories.eInnerCategory innerCategory,
            string author)
                : base(name, printDate, baseCategory, innerCategory)
        {
        }

        public string Author { get; set; }

        public override bool Equal(AbstractItem item)
        {
            Book tmpBook = item as Book;
            if (base.Equal(item) && Author == tmpBook.Author)
                return true;
            else
                return false;
        }
    }
}