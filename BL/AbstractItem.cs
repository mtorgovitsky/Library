using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BookLib
{
    public abstract class AbstractItem
    {
        public Guid ISBN { get; set; }

        public AbstractItem(string name, DateTime printDate)
        {
            ISBN = Guid.NewGuid();
            if (name != null)
                _name = name;
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
                _printDate = value;
            }
        }

        private DateTime _printDate;
        private string _name;
    }
}