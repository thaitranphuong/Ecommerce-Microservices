
using OrderService.Constants;

namespace OrderService.AsyncServices
{
    public interface IMessageProducer
    {
        void SendMessage<T>(EventType eventType, T data);
    }
}
