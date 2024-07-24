
using System.Threading.Tasks;

namespace CartService.AsyncServices
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string jsonMessage);
    }
}
