using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;

namespace WpfSharp.Controls
{
    public class SearchableTextBlock : TextBlock
    {
        #region Constructors
        // Summary:
        //     Initializes a new instance of the System.Windows.Controls.TextBlock class.
        public SearchableTextBlock()
        {
            //Binding binding = new Binding("HighlightableText");
            //binding.Source = this;
            //binding.Mode = BindingMode.TwoWay;
            //SetBinding(TextProperty, binding);
        }

        public SearchableTextBlock(Inline inline)
            : base(inline)
        {
        }
        #endregion

        #region Properties
        new private string Text
        {
            set
            {
                if (string.IsNullOrWhiteSpace(RegularExpression) || !IsValidRegex(RegularExpression))
                {
                    base.Text = value;
                    return;
                }

                Inlines.Clear();
                string[] split = Regex.Split(value, RegularExpression, RegexOptions);
                foreach (var str in split)
                {
                    Run run = new Run(str);
                    if (Regex.IsMatch(str, RegularExpression, RegexOptions))
                    {
                        run.Background = HighlightBackground;
                        run.Foreground = HighlightForeground;
                    }
                    Inlines.Add(run);
                }
            }
        }

        public string RegularExpression
        {
            get { return _RegularExpression; }
            set
            {
                _RegularExpression = value;
                Text = base.Text;
            }
        }
        private string _RegularExpression;


        #endregion

        #region Dependency Properties

        #region Search Words
        public List<string> SearchWords
        {
            get
            {
                if (null == (List<string>)GetValue(SearchWordsProperty))
                    SetValue(SearchWordsProperty, new List<string>());
                return (List<string>)GetValue(SearchWordsProperty);
            }
            set
            {
                SetValue(SearchWordsProperty, value);
                UpdateRegex();
            }
        }

        // Using a DependencyProperty as the backing store for SearchStringList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchWordsProperty =
            DependencyProperty.Register("SearchWords", typeof(List<string>), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(SearchWordsPropertyChanged)));

        public static void SearchWordsPropertyChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            if (stb == null)
                return;

            stb.UpdateRegex();
        }
        #endregion

        #region HighlightableText
        public event EventHandler OnHighlightableTextChanged;

        public string HighlightableText
        {
            get { return (string)GetValue(HighlightableTextProperty); }
            set { SetValue(HighlightableTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightableText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightableTextProperty =
            DependencyProperty.Register("HighlightableText", typeof(string), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(HighlightableTextChanged)));

        public static void HighlightableTextChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb.Text = stb.HighlightableText;

            // Raise the event by using the () operator.
            stb.OnHighlightableTextChanged?.Invoke(stb, null);
        }
        #endregion

        #region HighlightForeground
        public event EventHandler OnHighlightForegroundChanged;

        public Brush HighlightForeground
        {
            get
            {
                if ((Brush)GetValue(HighlightForegroundProperty) == null)
                    SetValue(HighlightForegroundProperty, Brushes.Black);
                return (Brush)GetValue(HighlightForegroundProperty);
            }
            set { SetValue(HighlightForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightForegroundProperty =
            DependencyProperty.Register("HighlightForeground", typeof(Brush), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(HighlightableForegroundChanged)));


        public static void HighlightableForegroundChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            // Raise the event by using the () operator.
            stb.OnHighlightForegroundChanged?.Invoke(stb, null);
        }
        #endregion

        #region HighlightBackground
        public event EventHandler OnHighlightBackgroundChanged;

        public Brush HighlightBackground
        {
            get
            {
                if ((Brush)GetValue(HighlightBackgroundProperty) == null)
                    SetValue(HighlightBackgroundProperty, Brushes.Yellow);
                return (Brush)GetValue(HighlightBackgroundProperty);
            }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(HighlightableBackgroundChanged)));


        public static void HighlightableBackgroundChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            // Raise the event by using the () operator.
            stb.OnHighlightBackgroundChanged?.Invoke(stb, null);
        }
        #endregion

        #region RegexOptions
        public event EventHandler OnRegexOptionsChanged;

        public RegexOptions RegexOptions
        {
            get { return (RegexOptions)GetValue(RegexOptionsProperty); }
            set { SetValue(RegexOptionsProperty, value); }
        }
        public static readonly DependencyProperty RegexOptionsProperty =
            DependencyProperty.Register("RegexOptions", typeof(RegexOptions), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(RegexOptionsChanged)));


        public static void RegexOptionsChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb.OnRegexOptionsChanged?.Invoke(stb, null);
        }
        #endregion

        #endregion

        #region Methods
        public void AddSearchString(string inString)
        {
            SearchWords.Add(inString);
            Update();
        }

        public void Update()
        {
            UpdateRegex();
        }

        public void RefreshHighlightedText()
        {
            Text = base.Text;
        }

        private void UpdateRegex()
        {
            string newRegularExpression = string.Empty;
            foreach (string s in SearchWords)
            {
                if (newRegularExpression.Length > 0)
                    newRegularExpression += "|";
                newRegularExpression += RegexWrap(s);
            }

            if (RegularExpression != newRegularExpression)
                RegularExpression = newRegularExpression;
        }

        public bool IsValidRegex(string inRegex)
        {
            if (string.IsNullOrEmpty(inRegex))
                return false;

            try
            {
                Regex.Match("", inRegex);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        private string RegexWrap(string inString)
        {
            return string.Format("(?={0})|(?<={0})", inString);
        }
        #endregion
    }
}
