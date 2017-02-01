using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BookLib
{
    public class Journal : AbstractItem
    {
        public Journal(string name, DateTime printDate) : base(name, printDate)
        {

        }
    }
}