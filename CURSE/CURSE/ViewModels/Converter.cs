using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace CURSE.Converter
{
    public class RichTextAndDateAndWindowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is RichTextBox rtb &&
                values[1] is DateTime date &&
                values[2] is Window window)
            {
                string text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text.Trim();
                return Tuple.Create(text, date, window);
            }

            return null!;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NicknameMatchToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Visibility.Collapsed;
            if (values[0] == null || values[1] == null) return Visibility.Collapsed;

            var currentUser = values[0]?.ToString()?.Trim();
            var noteUser = values[1]?.ToString()?.Trim();

            return currentUser == noteUser ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
