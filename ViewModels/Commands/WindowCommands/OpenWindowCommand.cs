using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Combiner_PDF.ViewModels.Commands
{
    public class OpenWindowCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            TypeInfo param = (TypeInfo)parameter;

            return param.BaseType == typeof(Window);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("TargetWindowType");

            TypeInfo param = (TypeInfo)parameter;

            if (param.BaseType != typeof(Window))
                throw new InvalidOperationException("parameter is not a Window type");

            Window window = Activator.CreateInstance(param) as Window;

            OpenWindow(window);
        }

        protected virtual void OpenWindow(Window window)
        {
            window.Show();
        }
    }
}
