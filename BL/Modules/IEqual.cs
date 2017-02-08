using BookLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Modules
{
    public interface IEqual
    {
        bool Equal(AbstractItem item);
    }
}
