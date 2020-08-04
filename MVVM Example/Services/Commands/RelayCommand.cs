using System;
using System.Windows.Input;

namespace MVVM_Example.ViewModel.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute; //execution logic
        private Func<object, bool> canExecute; //detects whether command can be executed


        public event EventHandler CanExecuteChanged //is called when conditions for whether command can be executed or not change
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}