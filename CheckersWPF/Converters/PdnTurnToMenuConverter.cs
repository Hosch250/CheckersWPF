using System;
using CheckersWPF.Facade;
using System.Windows.Data;
using System.Globalization;

namespace CheckersWPF.Converters
{
    public class PdnTurnToMenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var turn = (PdnTurn)value;
            var move = turn.WhiteMove ?? turn.BlackMove;

            var converter = new PdnMoveToDisplayStringConverter();
            return "☰ " + converter.Convert(move, null, null, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}