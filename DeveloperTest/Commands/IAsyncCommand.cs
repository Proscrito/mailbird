using System.Threading.Tasks;
using System.Windows.Input;

namespace DeveloperTest.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
