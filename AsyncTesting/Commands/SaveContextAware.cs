using AsyncTesting.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncTesting.Commands
{
    public class SaveContextAware : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Works because we never switch contexts
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            TheViewModel vm = parameter as TheViewModel;
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;

            //Remains context aware 
            string msg = await CallSomethingAsync(vm);
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            vm.UpdateDescription(msg);
        }

        private async Task<string> CallSomethingAsync(TheViewModel vm)
        {
            await Task.Delay(10);
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(1000);
            return $"Never switched contexts.";
        }
    }
}
