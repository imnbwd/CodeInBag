using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodeInBag.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public bool IsInversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = 0;
            if (value != null)
            {
                int.TryParse(value.ToString(), out num);
            }

            if (num > 0)
            {
                return IsInversed ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return IsInversed ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}