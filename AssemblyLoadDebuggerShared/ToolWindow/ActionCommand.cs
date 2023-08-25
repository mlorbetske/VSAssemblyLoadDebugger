using System;
using System.Windows.Input;

namespace ToolWindow
{
    internal class ActionCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool _canExecute;
        private Action<T> _act;
        private Func<T, bool> _func;

        internal ActionCommand(Action<T> act, Func<T, bool> func, bool initialCanExecute)
        {
            _act = act;
            _func = func ?? (o => true);
            _canExecute = initialCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool old = _canExecute;
            _canExecute = _func((T)parameter);

            if (old != _canExecute)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _act((T)parameter);
        }
    }

    internal class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool _canExecute;
        private Action _act;
        private Func<bool> _func;

        public static ICommand From(Action act, Func<bool> func = null, bool initialCanExecute = true)
        {
            return new ActionCommand(act, func, initialCanExecute);
        }

        public static ICommand From<T>(Action<T> act, Func<T, bool> func = null, bool initialCanExecute = true)
        {
            return new ActionCommand<T>(act, func, initialCanExecute);
        }

        internal ActionCommand(Action act, Func<bool> func, bool initialCanExecute)
        {
            _act = act;
            _func = func ?? (() => true);
            _canExecute = initialCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool old = _canExecute;
            _canExecute = _func();

            if (old != _canExecute)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _act();
        }

        internal static ICommand From(ICommand removeBreakConditionCommand, Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
