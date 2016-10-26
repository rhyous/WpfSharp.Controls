using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfSharp.Controls
{
    public class FileTextBox : TextBox
    {
        #region Private fields
        private bool _ChangedHandlerAdded;
        private bool _CreatedHandlerAdded;
        private bool _DeletedHandlerAdded;
        private bool _RenameHandlerAdded;
        #endregion

        #region constructor
        public FileTextBox()
        {
            AcceptsReturn = true;
            IsReadOnly = true;
            AutoScroll = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// If true, the FileTextBox will always scroll to the end when updated.
        /// </summary>
        public bool AutoScroll { get; set; }
        #endregion

        #region File Dependency Property
        public string File
        {
            get { return (string)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for File.
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(string), typeof(FileTextBox), new FrameworkPropertyMetadata(OnFilePropertyChanged));

        private static void OnFilePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var ftb = sender as FileTextBox;
            if (ftb == null || args.NewValue == null || string.IsNullOrWhiteSpace(args.NewValue.ToString()))
            {
                return;
            }
            var dir = GetDirectory(ref args);
            if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
            {
                ftb.Watcher.Path = Path.GetDirectoryName(args.NewValue.ToString());
                ftb.Watcher.Filter = Path.GetFileName(args.NewValue.ToString());
                ftb.AddEvents();
                ftb.Watcher.EnableRaisingEvents = true;
                ftb.UpdateFile();
            }
            else
            {
                ftb.Text = string.Empty;
            }
        }

        private static string GetDirectory(ref DependencyPropertyChangedEventArgs args)
        {
            try
            {
                return Path.GetDirectoryName(args.NewValue.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        private FileSystemWatcher Watcher
        {
            get { return _Watcher ?? (_Watcher = BuildWatcher()); }
        }
        private FileSystemWatcher _Watcher;

        private FileSystemWatcher BuildWatcher()
        {
            var watcher = new FileSystemWatcher { NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName };
            return watcher;
        }

        public void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(UpdateFileAction);
        }

        public void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(UpdateFileAction);
        }

        public void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                UpdateFile();
                EnableRaiseEvents();
            }));
        }

        public void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            Dispatcher.Invoke(UpdateFileAction);
        }

        private void EnableRaiseEvents()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (!Watcher.EnableRaisingEvents)
                    Watcher.EnableRaisingEvents = true;
            }));
        }

        private Action UpdateFileAction
        {
            get { return _UpdateFileAction ?? new Action(UpdateFile); }
        } private Action _UpdateFileAction;

        private void UpdateFile()
        {
            if (!System.IO.File.Exists(File))
            {

                Text = string.Empty;
                return;
            }
            using (var fs = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    Text = sr.ReadToEnd();
                    if (AutoScroll)
                        ScrollToEnd();
                }
            }
        }

        private void AddEvents()
        {
            if (!_CreatedHandlerAdded)
            {
                Watcher.Created += OnFileCreated;
                _CreatedHandlerAdded = true;
            }
            if (!_ChangedHandlerAdded)
            {
                Watcher.Changed += OnFileChanged;
                _ChangedHandlerAdded = true;
            }
            if (!_DeletedHandlerAdded)
            {
                Watcher.Deleted += OnFileDeleted;
                _DeletedHandlerAdded = true;
            }
            if (!_RenameHandlerAdded)
            {
                Watcher.Renamed += OnFileRenamed;
                _RenameHandlerAdded = true;
            }
        }
    }
}