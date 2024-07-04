using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.AsyncServices
{
    public class MessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName = "product-service-queue";

        private string _authServiceExchangeName = "AuthServiceExchange";

        public MessageConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("---> Event Recieved");
                var body = ea.Body;
                var json = Encoding.UTF8.GetString(body.ToArray());
                var message = JsonSerializer.Deserialize<string>(json);
                Console.WriteLine(message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "armadillo-01.rmq.cloudamqp.com",
                UserName = "ktsxxpei",
                Password = "NICIxab_JSihP4QQnfkJWZjqnCFFD8s9",
                VirtualHost = "ktsxxpei",
                Port = 5672
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _authServiceExchangeName, type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            _channel.QueueBind(queue: _queueName, 
                               exchange: _authServiceExchangeName, 
                               routingKey: "product-service");
            Console.WriteLine("--> Listening on message bus");
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabbitMQ Connection Shutdown");
        }
    }
}
