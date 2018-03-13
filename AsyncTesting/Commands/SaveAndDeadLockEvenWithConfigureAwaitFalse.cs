using AsyncTesting.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncTesting.Commands
{
    public class SaveAndDeadLockEvenWithConfigureAwaitFalse : ICommand
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

            //Blocks while CallSomethingAsync runs. 
            string msg =  CallSomethingAsync(vm).Result;
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            vm.UpdateDescription(msg);
        }

        private async Task<string> CallSomethingAsync(TheViewModel vm)
        {
            LibraryCode lc = new LibraryCode();
            string msg = await lc.BadEttiquette().ConfigureAwait(continueOnCapturedContext:false);
            return msg;
        }
    }
}
