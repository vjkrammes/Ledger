using System;
using System.Diagnostics;
using System.Windows.Input;

namespace LedgerClient.Infrastructure
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> ce)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = ce;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parm) => _canExecute == null || _canExecute(parm);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parm) => _execute(parm);
    }
}
