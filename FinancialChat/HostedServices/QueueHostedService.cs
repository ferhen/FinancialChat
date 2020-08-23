using FinancialChat.Services;
using FinancialChat.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialChat.HostedServices
{
    public class QueueHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IConnection _connection;
        private IModel _channel;

        public QueueHostedService(ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = loggerFactory.CreateLogger<QueueHostedService>();
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "financialChatQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                await HandleMessage(content);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(queue: "financialChatQueue",
                                         autoAck: true,
                                         consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(string content)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatroomHubService = scope.ServiceProvider.GetRequiredService<ChatroomHubService>();

            var contentArray = content.Split("|");
            var messageContent = contentArray[0];
            var chatroomId = contentArray[1];

            await chatroomHubService.PublishCommandResult(new MessageViewModel
            {
                Content = messageContent,
                UserName = "FinancialBot",
                CreatedOn = DateTimeOffset.Now
            }, chatroomId);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
