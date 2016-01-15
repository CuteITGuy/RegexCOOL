using System.Windows;
using System.Windows.Data;

namespace CB.RegexCOOL
{
    public class CountToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is int))
            {
                return DependencyProperty.UnsetValue;
            }

            var count = (int) value;
            return count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}