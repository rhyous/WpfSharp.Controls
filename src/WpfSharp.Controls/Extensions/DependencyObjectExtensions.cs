using System.Windows;
using System.Windows.Data;

namespace WpfSharp.Controls.Extensions
{
    internal static class DependencyObjectExtensions
    {
        internal static void SetBinding(this DependencyObject run, object source, DependencyProperty dProp, string propName = null)
        {
            Binding binding = new Binding(propName);
            binding.Source = source;
            BindingOperations.SetBinding(run, dProp, binding);
        }
    }
}
