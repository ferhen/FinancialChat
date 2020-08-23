using RabbitMQ.Client;
using System.Text;

namespace FinancialChat.Services
{
    public class QueuePublisherService
    {
        private readonly ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };

        public void Publish(string message)
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "financialBotQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "financialBotQueue",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
