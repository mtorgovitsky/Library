using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BL;

namespace BookLib
{
    /// <summary>
    /// Journal Class
    /// Inherits from Base AbstractItem class
    /// </summary>
    [Serializable]
    public class Journal : AbstractItem
    {
        /// <summary>
        /// Issue number of the Journal
        /// </summary>
        public int IssueNumber { get; set; }

        /// <summary>
        /// Ctor for The Journal
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="printDate">Print Date</param>
        /// <param name="baseCategory">Base Category</param>
        /// <param name="innerCategory">Inner Category</param>
        /// <param name="issueNumber">Issue number</param>
        public Journal(
            string name,
            DateTime printDate,
            Categories.eBaseCategory baseCategory,
            Categories.eInnerCategory innerCategory,
            int issueNumber)
                : base(name, printDate, baseCategory, innerCategory)
        {
            IssueNumber = issueNumber;
        }

        /// <summary>
        /// Override of the base method,
        /// adds Issue Comparison
        /// </summary>
        /// <param name="item">Abstract Item</param>
        /// <returns>Boolean true or false - found or not</returns>
        public override bool Equal(AbstractItem item)
        {
            Journal tmpJournal = item as Journal;
            if (base.Equal(item) && IssueNumber == tmpJournal.IssueNumber)
                return true;
            else
                return false;
        }
    }
}