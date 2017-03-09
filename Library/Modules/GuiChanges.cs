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
    /// <summary>
    /// Utilities static class for EASY GUI manipulations
    /// </summary>
    public static class GuiChanges
    {
        /// <summary>
        /// Show the element/s passed to parameters array
        /// </summary>
        /// <param name="values">One or many UIElement to show</param>
        public static void Show(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hide the element/s passed to parameters array
        /// </summary>
        /// <param name="values">One or many UIElement to hide</param>
        public static void Hide(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Enables the element/s passed to parameters array
        /// </summary>
        /// <param name="values">One or many UIElement to enable</param>
        public static void Enable(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.IsEnabled = true;
            }
        }

        /// <summary>
        /// Disables the element/s passed to parameters array
        /// </summary>
        /// <param name="values">One or many UIElement to disable</param>
        public static void Disable(params UIElement[] values)
        {
            foreach (var item in values)
            {
                item.IsEnabled = false;
            }
        }

        /// <summary>
        /// Fills the given ComboBox with eBaseCategory Enum values
        /// </summary>
        /// <param name="baseCombo">ComboBox UIElement control to fill</param>
        public static void FillComboWithBaseCategory(ComboBox baseCombo)
        {
            baseCombo.ItemsSource = Enum.GetValues(typeof(eBaseCategory));
        }

        /// <summary>
        /// Fills the given ComboBox with eInnerCategory Enum values
        /// </summary>
        /// <param name="baseCombo">ComboBox UIElement control to fill</param>
        public static void FillComboWithInnerCategory(ComboBox innerCombo, object baseItem)
        {
            innerCombo.ItemsSource = CategoriesDictionary[(eBaseCategory)baseItem];
        }
    }
}
