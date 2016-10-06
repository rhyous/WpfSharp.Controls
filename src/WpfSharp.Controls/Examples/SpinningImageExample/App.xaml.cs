using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SpinningImageExample.View;
using SpinningImageExample.ViewModel;

namespace SpinningImageExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindowViewModel viewmodel = new MainWindowViewModel();
            MainWindow main = new MainWindow();
            main.DataContext = viewmodel;
            main.Show();
        }
    }
}
