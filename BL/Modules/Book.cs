using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BL;

namespace BookLib
{
    /// <summary>
    /// Inherits from Base AbstractItem class
    /// </summary>
    [Serializable]
    public class Book : AbstractItem
    {
        /// <summary>
        /// Ctor for The Book
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="printDate">Print Date</param>
        /// <param name="baseCategory">Base Category</param>
        /// <param name="innerCategory">Inner Category</param>
        /// <param name="author">Author of the Book</param>
        public Book(
            string name,
            DateTime printDate,
            Categories.eBaseCategory baseCategory,
            Categories.eInnerCategory innerCategory,
            string author)
                : base(name, printDate, baseCategory, innerCategory)
        {
            Author = author;
        }

        /// <summary>
        /// Author of the book
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Override of the base method,
        /// adds Author Comparison
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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