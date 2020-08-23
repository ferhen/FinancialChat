using CsvHelper;
using FinancialBot.Helpers;
using FinancialBot.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinancialBot.Services
{
    public class FinancialService
    {
        private readonly static HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

        public async Task<string> GetStockQuote(string stockCode)
        {
            try
            {
                var response = await client.GetAsync(string.Format(baseUrl, stockCode));
                response.EnsureSuccessStatusCode();
                var dataStream = await response.Content.ReadAsStreamAsync();

                var stockQuotes = ParserHelper.GetDataFromStream<StockQuote>(dataStream);
                var currentQuoteMessage = stockQuotes
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault()
                    .GetQuoteMessage();

                return currentQuoteMessage;
            }
            catch (ReaderException)
            {
                return $"{stockCode} stock was not found";
            }
            catch (HttpRequestException)
            {
                return "Stock website could not be reached";
            }
            catch (Exception)
            {
                return "Unkown error";
            }
        }
    }
}
