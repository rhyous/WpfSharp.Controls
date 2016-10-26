using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfSharp.Controls
{
    public class TrimConverter : MarkupExtension, IValueConverter
    {
        #region MarkupExtension method

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _Converter ?? (_Converter = new TrimConverter());
        } private static TrimConverter _Converter;

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ReturnValueTrimmed(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ReturnValueTrimmed(value, parameter);
        }

        #endregion

        private static object ReturnValueTrimmed(object value, object parameter)
        {
            if (value == null) return null;
            var stringval = value as string;
            var trimChars = parameter as char[];
            return string.IsNullOrWhiteSpace(stringval) ? "" : (trimChars != null) ? stringval.Trim(trimChars) : stringval.Trim();
        }
    }
}
