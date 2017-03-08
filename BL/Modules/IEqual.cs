using BookLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Modules
{
    /// <summary>
    /// Interface to Inherit in The Base Class
    /// (AbstractItem), so all the Derived
    /// Classes will deploy "Equal" method
    /// and make custom comparison in each Derived class
    /// </summary>
    public interface IEqual
    {
        bool Equal(AbstractItem item);
    }
}
