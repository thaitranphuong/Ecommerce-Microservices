using InventoryService.Constants;

namespace InventoryService.AsyncServices
{
    public interface IMessageProducer
    {
        void SendMessage<T>(EventType eventType, T data);
    }
}
