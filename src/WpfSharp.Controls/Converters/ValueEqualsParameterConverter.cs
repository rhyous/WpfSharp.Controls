using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfSharp.Controls
{
    public class ValueEqualsParameterConverter : MarkupExtension, IValueConverter
    {
        #region MarkupExtension method
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _Converter ?? (_Converter = new ValueEqualsParameterConverter());
        } private static ValueEqualsParameterConverter _Converter;
        #endregion

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter) || DesignerProperties.GetIsInDesignMode(new DependencyObject());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
