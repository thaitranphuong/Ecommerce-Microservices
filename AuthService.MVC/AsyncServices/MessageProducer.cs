using AuthService.MVC.Constants;
using AuthService.MVC.Dtos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace AuthService.MVC.AsyncServices
{
    public class MessageProducer : IMessageProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        private string _authServiceExchangeName = "AuthServiceExchange";

        public MessageProducer (IConfiguration configuration)
        {
            try
            {
                _configuration = configuration;
                _factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"],
                    UserName = _configuration["RabbitMQ:UserName"],
                    Password = _configuration["RabbitMQ:Password"],
                    VirtualHost = _configuration["RabbitMQ:VirtualHost"],
                    Port = int.Parse(_configuration["RabbitMQ:Port"])
                };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: _authServiceExchangeName, type: ExchangeType.Direct);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage<T>(EventType eventType, T data)
        {
            var Event = EventGenerator.GenerateEvent(eventType);
            var message = new AsyncMessageDto<T>() { EventType = Event, Data = data };
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "AuthServiceExchange",
                                 routingKey: "product-service",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"---> We have sent the message");
        }
    }
}
