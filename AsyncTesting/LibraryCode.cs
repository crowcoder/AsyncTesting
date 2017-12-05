using System.Threading.Tasks;

namespace AsyncTesting
{
    public class LibraryCode
    {
        public async Task<string> BadEttiquette()
        {
            await Task.Delay(1000);   //No deadlock if we add this: .ConfigureAwait(false);
            return "PC LOAD LETTER";
        }
    }
}
