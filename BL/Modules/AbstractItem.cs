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
    /// <summary>
    /// Main Item Class
    /// </summary>
    [Serializable]
    public abstract class AbstractItem : IEqual
    {
        public string ItemType
        {
            get
            {
                //Return the string with the name of the object
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// If Item was borrowed or not
        /// </summary>
        public bool IsBorrowed { get; set; }

        /// <summary>
        /// Ctor with parameters
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="printDate">Pront Date</param>
        /// <param name="baseCategory">Base category</param>
        /// <param name="innerCategory">Inner category</param>
        public AbstractItem
            (
                string name,
                DateTime printDate,
                eBaseCategory baseCategory,
                eInnerCategory innerCategory
            )
        {
            ISBN = Guid.NewGuid();
            if (name != null)
                _name = name;
            if (printDate != null)
                _printDate = printDate;

            /////////////   CHECK THE CATEGORIES Validity  //////////////
            if (Categories.CategoriesDictionary.ContainsKey(baseCategory))
            {
                var catList = Categories.CategoriesDictionary.FirstOrDefault(p => p.Key == baseCategory);
                if (catList.Value.Contains(innerCategory))
                {
                    BaseCategory = baseCategory;
                    InnerCategory = innerCategory;
                }
            }
            /////////////   CHECK THE CATEGORIES Validity  //////////////

            PrintDate = printDate;
        }


        private DateTime _printDate;

        private string _name;

        public eBaseCategory BaseCategory { get; set; }

        private eInnerCategory _innerCategory;

        /// <summary>
        /// Getter and Setter From the Dictionary of Categories - 
        /// different Inner categories for each Base category
        /// </summary>
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

        /// <summary>
        /// Getter and Setter for Name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Getter and Setter for Print Date
        /// </summary>
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

        /// <summary>
        /// Custom Equal method
        /// </summary>
        /// <param name="item">AbstractItem to check</param>
        /// <returns>true or false - Equality result</returns>
        public virtual bool Equal(AbstractItem item)
        {
            if (Name == item.Name && BaseCategory == item.BaseCategory
                && InnerCategory == item.InnerCategory &&
                PrintDate == item.PrintDate)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ISBN Generated in Ctor by random GUID - read only
        /// </summary>
        public Guid ISBN { get; }
    }
}