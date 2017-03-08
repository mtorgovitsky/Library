using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static BL.Categories;

namespace Library.Modules
{
    public static class GuiChanges
    {
        public static void Show(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.Visibility = Visibility.Visible;
            }
        }

        public static void Hide(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.Visibility = Visibility.Hidden;
            }
        }

        public static void Enable(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.IsEnabled = true;
            }
        }

        public static void Disable(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.IsEnabled = false;
            }
        }

        public static void FillComboWithBaseCategory(ComboBox baseCombo)
        {
            baseCombo.ItemsSource = Enum.GetValues(typeof(eBaseCategory));
        }

        public static void FillComboWithInnerCategory(ComboBox innerCombo, object baseItem)
        {
            innerCombo.ItemsSource = CategoriesDictionary[(eBaseCategory)baseItem];
        }
    }
}
