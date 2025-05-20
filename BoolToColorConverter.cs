using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Collections;

namespace SmartCart
{
    class BoolToColorConverter : IValueConverter
    {
        public string SelectedColorResourceKey { get; set; } = "SelectedBorderColor";
        public string UnselectedColorResourceKey { get; set; } = "UnselectedBorderColor";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;

            var resourceKey = isSelected ? SelectedColorResourceKey : UnselectedColorResourceKey;

            if (Application.Current.Resources.TryGetValue(resourceKey, out var resource))
            {
                if (resource is SolidColorBrush brush)
                    return brush.Color;
            }

            return Colors.Transparent; // fallback if resource is not found
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
