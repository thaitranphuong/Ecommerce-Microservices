using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.MVC.AsyncServices
{
    public class MessageProducer : IMessageProducer
    {
        private string _authServiceExchangeName = "AuthServiceExchange";

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "armadillo-01.rmq.cloudamqp.com",
                UserName = "ktsxxpei",
                Password = "NICIxab_JSihP4QQnfkJWZjqnCFFD8s9",
                VirtualHost = "ktsxxpei",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _authServiceExchangeName, type: ExchangeType.Direct);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "AuthServiceExchange",
                                 routingKey: "product-service",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"---> We have sent message: {message}");
        }
    }
}
