using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CheckersWPF.Facade;

namespace CheckersWPF.Converters
{
    public class PlayerToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Player) value)
            {
                case Player.White:
                    return (string) parameter == nameof(Player.White) ? FontWeights.Bold : FontWeights.Normal;
                case Player.Black:
                    return (string)parameter == nameof(Player.Black) ? FontWeights.Bold : FontWeights.Normal;
                default:
                    return FontWeights.Bold;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}