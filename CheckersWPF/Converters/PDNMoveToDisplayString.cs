using System;
using CheckersWPF.Facade;
using System.Windows.Data;
using System.Globalization;

namespace CheckersWPF.Converters
{
    public class PdnMoveToDisplayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var move = (PdnMove)value;

            if (move == null) { return string.Empty; }
            if (move.Move.Count <= 3) { return move.DisplayString; }
            return move.Move[0] + "…" + move.Move[move.Move.Count - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value;
    }
}