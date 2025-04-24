using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SmartCart
{
    public class QuantityDisplayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString(); // Convert int to string
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int result))
                return result;
            return 1; // fallback
        }
    }
}


