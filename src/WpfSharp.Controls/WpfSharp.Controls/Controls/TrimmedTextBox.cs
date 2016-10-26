using System.Windows.Controls;

namespace WpfSharp.Controls
{
    public class TrimmedTextBox : TextBox
    {
        public TrimmedTextBox()
        {
            LostFocus += TrimOnLostFocus;
        }

        void TrimOnLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var trimTextBox = sender as TrimmedTextBox;
            if (trimTextBox != null)
                trimTextBox.Text = trimTextBox.Text.Trim();
        }
    }
}
