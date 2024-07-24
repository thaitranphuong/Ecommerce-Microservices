
using Microsoft.Extensions.Configuration;
using OrderService.Constants;
using OrderService.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace OrderService.AsyncServices
{
    public class MessageProducer : IMessageProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        private string _orderServiceExchangeName = "OrderServiceExchange";

        public MessageProducer (IConfiguration configuration)
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
            _channel.ExchangeDeclare(exchange: _orderServiceExchangeName, type: ExchangeType.Direct);
        }

        public void SendMessage<T>(EventType eventType, T data)
        {

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.ConfirmSelect();

            _channel.BasicAcks += (sender, ea) =>
            {
                Console.WriteLine($"Message with delivery tag {ea.DeliveryTag} has been confirmed.");
            };

            _channel.BasicNacks += (sender, ea) =>
            {
                Console.WriteLine($"Message with delivery tag {ea.DeliveryTag} has been negatively acknowledged.");
            };

            try
            {
                var Event = EventGenerator.GenerateEvent(eventType);
                var message = new AsyncMessageDto<T>() { EventType = Event, Data = data };
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);
                _channel.BasicPublish(exchange: _orderServiceExchangeName,
                                 routingKey: "cart-service",
                                 basicProperties: null,
                                 body: body);
                Console.WriteLine($"---> We have sent the message");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
