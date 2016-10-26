using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfSharp.Controls
{
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        #region MarkupExtension method

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _Converter ?? (_Converter = new BoolToVisibilityConverter());
        } private static BoolToVisibilityConverter _Converter;

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;

            bool reverse = parameter.ToString().Contains("Reverse");
            bool hidden = parameter.ToString().Contains("Hidden");
            return (bool)value != reverse ? Visibility.Visible : (hidden) ? Visibility.Hidden : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool reverse = parameter.ToString().Contains("Reverse");
            return ((Visibility)value == Visibility.Visible) ? !reverse : reverse;
        }

        #endregion
    }
}
