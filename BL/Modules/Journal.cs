using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BL;

namespace BookLib
{
    public class Journal : AbstractItem
    {
        public int IssueNumber { get; set; }

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