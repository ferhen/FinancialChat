using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FinancialBot.Helpers
{
    public static class ParserHelper
    {
        private readonly static CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true
        };

        public static IEnumerable<T> GetDataFromStream<T>(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var csvReader = new CsvReader(streamReader, configuration);

            return csvReader.GetRecords<T>()?.ToList();
        }
    }
}
