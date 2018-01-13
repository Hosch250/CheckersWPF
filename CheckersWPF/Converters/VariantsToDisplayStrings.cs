using System;
using System.Collections.Generic;
using System.Linq;
using CheckersWPF.Facade;
using System.Windows.Data;
using System.Globalization;

namespace CheckersWPF.Converters
{
    public class VariantsToDisplayStringsConverter : IValueConverter
    {
        private string VariantToString(Variant variant)
        {
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

        private Variant StringToVariant(string str)
        {
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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var variants = (IEnumerable<Variant>)value;
            return variants.Select(VariantToString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strings = (IEnumerable<string>)value;
            return strings.Select(StringToVariant);
        }
    }
}