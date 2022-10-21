using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Combiner_PDF.ViewModels.Commands.WindowCommands
{
    public class ShowDialogCommand : OpenWindowCommand
    {
        protected override void OpenWindow(Window window)
        {
            window.ShowDialog();
        }
    }
}
