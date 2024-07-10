using AuthService.MVC.Constants;

namespace AuthService.MVC.AsyncServices
{
    public interface IMessageProducer
    {
        void SendMessage<T>(EventType eventType, T data);
    }
}
