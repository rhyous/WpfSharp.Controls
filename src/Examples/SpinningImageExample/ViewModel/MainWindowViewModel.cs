using MVVM;
using System.ComponentModel;
using System.Windows.Input;

namespace SpinningImageExample.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string ButtonText { get { return IsExecuting ? "Stop" : "Spin"; } }

        public bool IsExecuting
        {
            get { return _IsExecuting; }
            set { SetProperty(ref _IsExecuting, value); }
        } private bool _IsExecuting;


        public ICommand ButtonClickCommand
        {
            get { return _PropertyCommand ?? (_PropertyCommand = new RelayCommand(f => DoCommand(), b => DoCommandCanClick())); }
            set { SetProperty(ref _PropertyCommand, ButtonClickCommand); }
        } private ICommand _PropertyCommand;

        private void DoCommand()
        {
            IsExecuting = !IsExecuting;
        }

        private bool DoCommandCanClick()
        {
            return true;
        }
    }
}
