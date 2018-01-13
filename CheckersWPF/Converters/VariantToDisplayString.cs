using System;
using CheckersWPF.Facade;
using System.Windows.Data;
using System.Globalization;

namespace CheckersWPF.Converters
{
    public class VariantToDisplayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var variant = (Variant)value;
            switch (variant)
            {
                case Variant.AmericanCheckers:
                    return "American Checkers";
                case Variant.PoolCheckers:
                    return "Pool Checkers";
                default:
                    throw new ArgumentException(nameof(variant));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            switch (str)
            {
                case "American Checkers":
                    return Variant.AmericanCheckers;
                case "Pool Checkers":
                    return Variant.PoolCheckers;
                default:
                    throw new ArgumentException(nameof(str));
            }
        }
    }
}