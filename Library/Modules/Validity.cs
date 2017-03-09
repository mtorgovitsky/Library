using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modules
{
    /// <summary>
    /// Static Class, which contains useful validation functions
    /// </summary>
    public static class Validity
    {
        /// <summary>
        /// Checks if the given string is positive integer or not
        /// </summary>
        /// <param name="s">String to check</param>
        /// <returns>true or false relative to check result</returns>
        public static bool PositiveInteger(string s)
        {
            int x;
            if (int.TryParse(s, out x) && x > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the given string contains any useful data
        /// </summary>
        /// <param name="s">String to check</param>
        /// <returns>true or false relative to check result</returns>
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
