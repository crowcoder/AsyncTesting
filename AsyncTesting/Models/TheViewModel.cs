using AsyncTesting.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;

namespace AsyncTesting.Models
{
    public class TheViewModel : INotifyPropertyChanged
    {
        public TheViewModel()
        {
            CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            Quantity = 10;
            ProductName = "Balloon";
            ProductDescription = "Full of air";

            SaveCrossContextCmd = new SaveCrossContext();
            SaveContextAware = new SaveContextAware();
            SaveSplitCrossCtxCmd = new SaveSplitCrossContext();
            SaveAndDeadLockCmd = new SaveAndDeadLock();
            SaveWithNoDeadlockCmd = new SaveWithNoDeadlock();
            SaveCompletedTaskCmd = new SaveAndDeadLockEvenWithConfigureAwaitFalse();
        }

        public SaveCrossContext SaveCrossContextCmd { get; }
        public SaveContextAware SaveContextAware { get; }
        public SaveSplitCrossContext SaveSplitCrossCtxCmd { get; }
        public SaveAndDeadLock SaveAndDeadLockCmd { get; }
        public SaveWithNoDeadlock SaveWithNoDeadlockCmd { get; }
        public SaveAndDeadLockEvenWithConfigureAwaitFalse SaveCompletedTaskCmd { get; }

        /// <summary>
        /// this is a refernece to a TextBox UI component on the window.
        /// </summary>
        public TextBox DescriptionTextBox { get; set; }

        private int _CurrentThreadID;

        public int CurrentThreadID
        {
            get { return _CurrentThreadID; }
            set
            {
                _CurrentThreadID = value;
                OnPropertyChanged();
            }
        }


        private int _Quantity;
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                OnPropertyChanged();
            }
        }

        private string _ProductName;
        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                _ProductName = value;
                OnPropertyChanged();
            }
        }

        private string _ProductDescription;
        public string ProductDescription
        {
            get { return _ProductDescription; }
            set
            {
                _ProductDescription = value;
                OnPropertyChanged();
            }
        }

        public void UpdateDescription(string msg)
        {
            this.DescriptionTextBox.Text = msg;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
