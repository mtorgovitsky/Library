using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Categories
    {
        public Dictionary<eBaseCategory, List<eInnerCategory>> CategoriesDictionary =
            new Dictionary<eBaseCategory, List<eInnerCategory>>
            {
                {eBaseCategory.Kids,
                    new List<eInnerCategory>
                    {
                        eInnerCategory.Jewish,
                        eInnerCategory.Comics
                    }
                },
                {eBaseCategory.Study,
                    new List<eInnerCategory>
                    {
                        eInnerCategory.Jewish,
                        eInnerCategory.Tech,
                        eInnerCategory.Math,
                        eInnerCategory.Phisics,
                        eInnerCategory.Medicine
                    }
                },
                {eBaseCategory.Reading,
                    new List<eInnerCategory>
                    {
                        eInnerCategory.Jewish,
                        eInnerCategory.Novella,
                        eInnerCategory.Roman
                    }
                },
                {eBaseCategory.Cooking,
                    new List<eInnerCategory>
                    {
                        eInnerCategory.Soups,
                        eInnerCategory.Meat,
                        eInnerCategory.Fish,
                        eInnerCategory.Desserts,
                    }
                }
            };

        public enum eBaseCategory
        {
            Kids,
            Study,
            Reading,
            Cooking
        }

        public enum eInnerCategory
        {
            Jewish,
            Comics,
            Drama,
            Novella,
            Roman,
            Tech,
            Math,
            Medicine,
            Phisics,
            Soups,
            Desserts,
            Meat,
            Fish
        }
    }
}
