using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialBot.Services
{
    public class QueueService : BackgroundService, IDisposable
    {
        private readonly ILogger<QueueService> _logger;
        private readonly FinancialService financialService = new FinancialService();
        private readonly ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq" };

        private IModel consumerChannel;
        private IModel publisherChannel;

        private bool disposedValue;

        public QueueService(ILogger<QueueService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.WaitAll(
                Task.Factory.StartNew(() => StartConsumerQueue(stoppingToken)),
                Task.Factory.StartNew(() => StartPublisherQueue(stoppingToken))
            );

            return Task.CompletedTask;
        }

        private void StartConsumerQueue(CancellationToken token)
        {
            using var connection = factory.CreateConnection();
            consumerChannel = connection.CreateModel();
            consumerChannel.QueueDeclare(queue: "financialBotQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(consumerChannel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Received > {message}");

                var messageArray = message.Split("|");
                var stockCode = messageArray[0];
                var chatroomId = messageArray[1];

                var stockQuote = await financialService.GetStockQuote(stockCode);
                _logger.LogInformation($"Sent > {stockQuote}");

                PublishMessage($"{stockQuote}|{chatroomId}");
            };
            consumerChannel.BasicConsume(queue: "financialBotQueue",
                                 autoAck: true,
                                 consumer: consumer);

            token.WaitHandle.WaitOne();
        }

        private void StartPublisherQueue(CancellationToken token)
        {
            using var connection = factory.CreateConnection();
            publisherChannel = connection.CreateModel();
            publisherChannel.QueueDeclare(queue: "financialChatQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            token.WaitHandle.WaitOne();
        }

        private void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            publisherChannel.BasicPublish(exchange: "",
                                 routingKey: "financialChatQueue",
                                 basicProperties: null,
                                 body: body);
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    consumerChannel?.Dispose();
                    publisherChannel?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
