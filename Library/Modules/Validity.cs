using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modules
{
    public static class Validity
    {
        public static bool PositiveInteger(string s)
        {
            int x;
            if (int.TryParse(s, out x) && x > 0)
                return true;
            else
                return false;
        }

        public static bool StringOK(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
