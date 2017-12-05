using AsyncTesting.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncTesting.Commands
{
    public class SaveSplitCrossContext : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Work is done on default context but is resumed on the 
        /// captured context.
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            TheViewModel vm = parameter as TheViewModel;
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            
            //No ConfigureAwait False here! The work in that method DOES
            //operate on a different conext but execution will resume
            //on the original context so we can access the GUI thread.
            string msg = await CallSomethingAsync(vm);
                       
            vm.UpdateDescription(msg);
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        private async Task<string> CallSomethingAsync(TheViewModel vm)
        {
            //All this work is context UN-aware and will switch to a separate
            //context.
            await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: false);
            return $"Work is on default context but resumes on captured.";
        }
    }
}
