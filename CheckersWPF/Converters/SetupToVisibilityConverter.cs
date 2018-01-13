using CheckersWPF.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CheckersWPF.Converters
{
    public class SetupToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (Setup) value == Setup.Default ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (Visibility)value == Visibility.Collapsed ? Setup.Default : Setup.FromPosition;
    }
}