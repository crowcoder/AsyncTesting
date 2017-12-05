using AsyncTesting.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncTesting.Commands
{
    public class SaveWithNoDeadlock : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Deadlocks
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            TheViewModel vm = parameter as TheViewModel;
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;

            //Blocks while CallSomethingAsync runs. CallSomethingAsync
            //cannot resume on the context because .Result is blocking.
            string msg =  CallSomethingAsync(vm).Result;
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            vm.UpdateDescription(msg);
        }

        private async Task<string> CallSomethingAsync(TheViewModel vm)
        {
            await Task.Delay(10).ConfigureAwait(continueOnCapturedContext:false);
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: false);
            return $"No deadlock because blocking call is on different context.";
        }
    }
}
