using IdentityService.Constants;

namespace IdentityService.AsyncServices
{
    public interface IMessageProducer
    {
        void SendMessage<T>(EventType eventType, T data);
    }
}
