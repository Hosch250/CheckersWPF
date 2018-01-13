using System;
using CheckersWPF.Facade;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace CheckersWPF.Converters
{
    public class PdnMoveToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (PdnMove) value == null ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}