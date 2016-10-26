using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;

namespace WpfSharp.Controls
{
    /// <summary>
    /// Interaction logic for FileSelectorControl.xaml
    /// </summary>
    public partial class FileSelector
    {
        public FileSelector()
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(InitialDirectory))
                InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        }

        #region File DependencyProperty
        public string File
        {
            get { return (string)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }

        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        #endregion

        #region Label DependencyProperty
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        #endregion

        #region ButtonLabel DependencyProperty
        public string ButtonLabel
        {
            get { return (string)GetValue(ButtonLabelProperty); }
            set { SetValue(ButtonLabelProperty, value); }
        }

        public static readonly DependencyProperty ButtonLabelProperty =
            DependencyProperty.Register("ButtonLabel", typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        #endregion

        #region InitialDirectory DependencyProperty
        public string InitialDirectory
        {
            get { return (string)GetValue(InitialDirectoryProperty); }
            set { SetValue(InitialDirectoryProperty, value); }
        }

        public static readonly DependencyProperty InitialDirectoryProperty =
            DependencyProperty.Register("InitialDirectory", typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        #endregion

        #region FileFilter DependencyProperty
        public string FileFilter
        {
            get { return (string)GetValue(FileFilterProperty); }
            set { SetValue(FileFilterProperty, value); }
        }

        public static readonly DependencyProperty FileFilterProperty =
            DependencyProperty.Register("FileFilter", typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        #endregion

        #region FileExtension DependencyProperty
        public string FileExtension
        {
            get { return (string)GetValue(FileExtensionProperty); }
            set { SetValue(FileExtensionProperty, value); }
        }

        public static readonly DependencyProperty FileExtensionProperty =
            DependencyProperty.Register("FileExtension", typeof(string), typeof(FileSelector), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        #endregion


        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".xml",
                Filter = FileFilter,
                InitialDirectory = InitialDirectory
            };
            var result = dlg.ShowDialog();
            if (result == true)
            {
                TextBoxFile.Text = dlg.FileName;
            }
        }
    }
}
