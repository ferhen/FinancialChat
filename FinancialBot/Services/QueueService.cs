using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialBot.Services
{
    public class QueueService : IDisposable
    {
        private readonly FinancialService financialService = new FinancialService();
        private readonly ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        private IModel consumerChannel;
        private IModel publisherChannel;

        private bool disposedValue;

        public void StartQueue(CancellationToken token)
        {
            Task.WaitAll(
                Task.Factory.StartNew(() => StartConsumerQueue(token)),
                Task.Factory.StartNew(() => StartPublisherQueue(token))
            );
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
                Console.WriteLine("I> Received {0}", message);

                var messageArray = message.Split("|");
                var stockCode = messageArray[0];
                var chatroomId = messageArray[1];

                var stockQuote = await financialService.GetStockQuote(stockCode);
                Console.WriteLine($"O> {stockQuote}");

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
