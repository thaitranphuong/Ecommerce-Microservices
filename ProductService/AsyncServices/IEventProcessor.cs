
namespace ProductService.AsyncServices
{
    public interface IEventProcessor
    {
        public void ProcessEvent(string jsonMessage);
    }
}
