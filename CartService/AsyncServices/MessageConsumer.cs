using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CartService.AsyncServices
{
    public class MessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Chỉ cho phép một tác vụ đồng thời

        private string _queueName = "cart-service-queue";

        private string _authServiceExchangeName = "OrderServiceExchange";

        public MessageConsumer(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ModuleHandle, ea) =>
            {
                await _semaphore.WaitAsync(stoppingToken); // Chờ đợi nếu có tác vụ khác đang xử lý
                try
                {
                    Console.WriteLine("---> Event Recieved");
                    var body = ea.Body;
                    var json = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(json);
                    await _eventProcessor.ProcessEvent(json);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    bool basicNackSuccess = false;
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    while (basicNackSuccess == false)
                    {
                        try
                        {
                            _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                            basicNackSuccess = true;
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine(ex1.Message);
                            basicNackSuccess = false;
                        }
                    }
                }
                finally
                {
                    if (_semaphore.CurrentCount == 0)
    {
        _semaphore.Release();
    }
                }
            };
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"],
                VirtualHost = _configuration["RabbitMQ:VirtualHost"],
                Port = int.Parse(_configuration["RabbitMQ:Port"])
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            _channel.ExchangeDeclare(exchange: _authServiceExchangeName, type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            _channel.QueueBind(queue: _queueName, 
                               exchange: _authServiceExchangeName, 
                               routingKey: "cart-service");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
