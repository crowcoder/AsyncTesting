using AsyncTesting.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncTesting.Commands
{
    public class SaveCrossContext : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Blows up because trying to access GUI from wrong context.
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            TheViewModel vm = parameter as TheViewModel;
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;

            //Switching to default context here.
            string msg = await CallSomethingAsync(vm).ConfigureAwait(continueOnCapturedContext: false);
            
            //Still on default context, no access to GUI thread. Boom.
            vm.UpdateDescription(msg);
        }

        private async Task<string> CallSomethingAsync(TheViewModel vm)
        {
            await Task.Delay(10).ConfigureAwait(continueOnCapturedContext:false);
            vm.CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(1000);
            return "You will never see this message";
        }
    }
}
