﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CheckersWPF.Converters
{
    public class IntegerToListItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (int)value + ".";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value;
    }
}