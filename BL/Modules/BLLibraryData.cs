using BookLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Modules
{
    [Serializable]
    public class BLLibraryData
    {
        public List<AbstractItem> Items { get; set; }
        public User SuperAdmin { get; set; }
        public List<User> Users { get; set; }

        public void SaveData(BLLibraryData data)
        {
            Data.DBData.Serialize(data);
        }

        public BLLibraryData GetBLData()
        {
            var data = Data.DBData.DeSerialize<BLLibraryData>();
            return data;
        }
    }
}
