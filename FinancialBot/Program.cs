using FinancialBot.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            var queueService = new QueueService();

            Task.Run(() =>
            {
                queueService.StartQueue(token);
            }, token);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            cancellationTokenSource.Cancel();
        }
    }
}
