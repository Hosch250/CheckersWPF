﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CheckersWPF.Converters
{
    public class BooleanToHiddenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool) value ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (Visibility)value == Visibility.Collapsed;
    }
}