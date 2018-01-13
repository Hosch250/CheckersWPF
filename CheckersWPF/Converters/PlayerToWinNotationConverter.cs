using System;
using System.Globalization;
using System.Windows.Data;
using CheckersWPF.Facade;

namespace CheckersWPF.Converters
{
    public class PlayerToWinNotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var player = (Player?)value;

            if (!player.HasValue) { return "*"; }

            return player.Value == Player.Black ? "0 - 1" : "1 - 0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}