using System.Windows;
using System.Windows.Media.Animation;

namespace WpfSharp.Controls
{
    /// <summary>
    /// Interaction logic for SpinningImage.xaml
    /// </summary>
    public partial class SpinningImage
    {
        public SpinningImage()
        {
            InitializeComponent();
        }

        public Storyboard Spinner
        {
            get { return _Spinner ?? (_Spinner = (Storyboard)FindResource("Spin360")); }
        }
        private Storyboard _Spinner;

        public bool SpinnerState
        {
            get { return (bool)GetValue(SpinnerStateProperty); }
            set { SetValue(SpinnerStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpinnerState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpinnerStateProperty =
            DependencyProperty.Register("SpinnerState", typeof(bool), typeof(SpinningImage), new UIPropertyMetadata(false, OnSpinnerStatePropertyChanged));

        public static void OnSpinnerStatePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var view = source as SpinningImage;
            if (view == null) return;
            if ((bool)e.NewValue)
                view.Spinner.Begin();
            else
                view.Spinner.Stop();
        }
    }
}
