using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace CB.RegexCOOL
{
    public class CountToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                return DependencyProperty.UnsetValue;
            }

            var count = (int)value;

            if (count == 0)
            {
                return "";
            }

            string singularForm, pluralForm;
            GetSingularPluralForms(parameter, out singularForm, out pluralForm);

            const string format = "{0} {1}";
            return string.Format(format, count, count > 1 ? pluralForm : singularForm);
        }

        private static void GetSingularPluralForms(object parameter, out string singularForm, out string pluralForm)
        {
            singularForm = "item";
            pluralForm = "items";
            var paraString = parameter as string;

            if (string.IsNullOrWhiteSpace(paraString))
            {
                return;
            }

            const string pattern = @"(\w+)\|(\w+)";
            var matchedGroup = Regex.Match(paraString, pattern).Groups;

            if (matchedGroup.Count > 1)
            {
                singularForm = matchedGroup[1].Value;
            }

            pluralForm = matchedGroup.Count > 2 ? matchedGroup[2].Value : singularForm + "s";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}