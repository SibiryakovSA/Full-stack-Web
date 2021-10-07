using API.Models.DataClasses;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public static class DataWriter
    {
        private static string dataFilePass;
        static DataWriter()
        {
            dataFilePass = Environment.GetEnvironmentVariable("DataFilePath");
        }

        // Изменить запись в csv файле с данными 
        public static bool EditInvoiceById(int InvoiceNumber, Invoice data)
        {
            var result = false;
            // Получаем список всех записей и нужную запись
            var invoices = DataReader.GetAllInvoicesFromCsvFile();
            var invoice = invoices.First(e => e.InvoiceNumber == InvoiceNumber);

            // Если по какой либо причине запись не существует (хотя обязана) то завершаем метод
            if (invoice is null)
                return result;

            // Копируем значение полей
            data.CopyTo(invoice);

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    Encoding = Encoding.UTF8
                };
                // Перезаписываем файл
                using (var streamWriter = new StreamWriter(dataFilePass))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, config))
                    {
                        //csvWriter.WriteRecords(invoices);

                        foreach (var inv in invoices)
                        {
                            WriteOneInvoice(inv, csvWriter);
                            if (inv != invoices.Last())
                            {
                                csvWriter.NextRecord();
                            }
                        }
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось записать данные в файл, текст ошибки:\n" + e.Message);
            }

            return result;
        }

        // Добавить запись в csv файл с данными
        public static bool PushInvoiceToCsvFile(Invoice invoice)
        {
            var result = false;
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    Encoding = Encoding.UTF8
                };
                // Добавляем запись в файл
                using (var streamWriter = new StreamWriter(dataFilePass, true, Encoding.UTF8))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, config))
                    {
                        csvWriter.NextRecord();
                        WriteOneInvoice(invoice, csvWriter);
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось добавить запись в файл, текст ошибки:\n" + e.Message);
            }

            return result;
        }

        // Записывает 1 счет в строку (без перехода на новую строку), для внутреннего использования (внтури класса)
        static void WriteOneInvoice(Invoice invoice, CsvWriter csvWriter)
        {
            // Записываем поля в словарь, потом для каждого ключа записываем поле
            var tempDict = new Dictionary<string, string>();
            tempDict.Add("CreationDate", invoice.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
            tempDict.Add("EditionDate", invoice.EditionDate == new DateTime() ? "" : invoice.EditionDate.ToString("yyyy-MM-dd HH:mm:ss"));
            tempDict.Add("InvoiceNumber", invoice.InvoiceNumber.ToString());
            tempDict.Add("ProcessingStatus", invoice.ProcessingStatus.ToString("d"));
            tempDict.Add("Balance", invoice.Balance.ToString());
            tempDict.Add("PaymentMethod", invoice.PaymentMethod.ToString("d"));

            foreach (var s in tempDict)
            {
                csvWriter.WriteField(s.Value);
            }
        }
    }
}
