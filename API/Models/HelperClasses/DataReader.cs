using API.Models.DataClasses;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using CsvHelper.Configuration;
using System.Text;

namespace API.Models
{
    public static class DataReader
    {
        private static string dataFilePass;
        static DataReader()
        {
            dataFilePass = Environment.GetEnvironmentVariable("DataFilePath");
        }

        // Получить все записи из csv файла
        public static IEnumerable<Invoice> GetAllInvoicesFromCsvFile()
        {
            List<Invoice> result = null;
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ";"
                };
                using (var streamReader = new StreamReader(dataFilePass))
                {
                    using (var csvReader = new CsvReader(streamReader, config))
                    {
                        result = new List<Invoice>() { };

                        while (csvReader.Read())
                        {
                            var tempInvoice = new Invoice()
                            {
                                CreationDate = csvReader.GetField<DateTime>(0),
                                EditionDate = csvReader.GetField(1) == "" ? new DateTime() : DateTime.Parse(csvReader.GetField(1)),
                                InvoiceNumber = csvReader.GetField<int>(2),
                                ProcessingStatus = csvReader.GetField<ProcessingStatus>(3),
                                Balance = csvReader.GetField<double>(4),
                                PaymentMethod = csvReader.GetField<PaymentMethod>(5)
                            };
                            result.Add(tempInvoice);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось прочитать данные из файла, текст ошибки:\n" + e.Message);
            }

            return result;
        }

        public static Invoice GetInvoiceByIdFromCsvFile(int invoiceNumber)
        {
            // Из списка всех счетов находим тот, где id соответствет
            var invoices = GetAllInvoicesFromCsvFile();
            var result = invoices.First(e => e.InvoiceNumber == invoiceNumber);

            // Если не существует, то вернет null
            return result;
        }
    }
}
