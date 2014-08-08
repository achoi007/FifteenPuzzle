using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FifteenPuzzleLib
{
    public class RelayCommand : ICommand
    {
        private Action<object> mExecCB;
        private Predicate<object> mCanExecCB;

        public RelayCommand(Action<object> execCB, Predicate<object> canExecCB = null)
        {
            mExecCB = execCB;
            mCanExecCB = canExecCB;
        }

        public bool CanExecute(object parameter)
        {
            if (mCanExecCB == null)
            {
                return true;
            }
            else
            {
                return mCanExecCB(parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            mExecCB(parameter);
        }
    }
}
