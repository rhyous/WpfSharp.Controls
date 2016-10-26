using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfSharp.Controls
{
    public class DigitBox : TextBox
    {
        #region Constructor
        public DigitBox()
        {
            DataObject.AddPastingHandler(this, OnPaste);
        }
        #endregion

        #region Properties
        new public string Text
        {
            get { return base.Text; }
            set { base.Text = LeaveOnlyNumbers(value); }
        }

        #endregion

        #region Functions
        private bool IsNumberKey(Key inKey)
        {
            return ((inKey >= Key.D0 && inKey <= Key.D9) || (inKey >= Key.NumPad0 && inKey <= Key.NumPad9));
        }

        private bool IsActionKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || inKey == Key.Return
                || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) || Keyboard.Modifiers.HasFlag(ModifierKeys.Control)
                || Keyboard.Modifiers.HasFlag(ModifierKeys.Windows);
        }

        private string LeaveOnlyNumbers(string inString)
        {
            return new string(inString.Where(c => char.IsDigit(c)).ToArray());
        }
        #endregion

        #region Event Functions
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = !IsNumberKey(e.Key) && !IsActionKey(e.Key);
            if (!e.Handled)
                base.OnKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            var numberText = LeaveOnlyNumbers(Text);
            if (base.Text != numberText)
                base.Text = numberText;
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText)
                return;
            var caretIndex = CaretIndex;
            var pastedText = e.DataObject.GetData(typeof(string)) as string; 
            var text = $"{Text.Substring(0, SelectionStart)}{Text.Substring(SelectionStart + SelectionLength)}";
            var value = $"{text.Substring(0, caretIndex) + pastedText}{text.Substring(caretIndex)}";
            Text = value;
            CaretIndex = Math.Min(Text.Length, caretIndex + pastedText.Length);
            e.CancelCommand();
        }
        #endregion
    }
}