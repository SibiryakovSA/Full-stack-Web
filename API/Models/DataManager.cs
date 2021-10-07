using API.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Models
{
    public class DataManager
    {
        /// <summary>
        /// Получение списка всех  счетов 
        /// orderAsc: true - сортировка по возрастанию, false - по убыванию, null - без сортировки 
        /// pageParams - параметры страницы
        /// </summary>

        public List<Invoice> GetInvoices(bool? orderAsc = null, PageParams pageParams = null)
        {
            // Параметры фильтрации
            var filteringParams = pageParams is null ? null : pageParams.FilteringParams;

            // Список всех счетов из Csv
            var result = DataReader.GetAllInvoicesFromCsvFile();

            // Если задана фильтрация
            if (!(filteringParams is null))
            {
                if (filteringParams.InvoiceNumberMax != -1)
                    result = result.Where(e => e.InvoiceNumber < filteringParams.InvoiceNumberMax);

                if (filteringParams.InvoiceNumberMin != -1)
                    result = result.Where(e => e.InvoiceNumber >= filteringParams.InvoiceNumberMin);

                if (filteringParams.paymentMethod != PaymentMethod.Undefind)
                    result = result.Where(e => e.PaymentMethod == filteringParams.paymentMethod);

                if (filteringParams.processingStatus != ProcessingStatus.Undefind)
                    result = result.Where(e => e.ProcessingStatus == filteringParams.processingStatus);
            }


            // Если задана сортировка
            if (orderAsc == true)
                result = result.OrderBy((e) => e.InvoiceNumber);
            else if (orderAsc == false)
                result = result.OrderByDescending((e) => e.InvoiceNumber);

            // Если переданы параметры страницы
            if (!(pageParams is null))
            {
                var startNumber = pageParams.StartNumber;
                var finishNumber = pageParams.FinishNumber;
                // Если корректно задан диапазон (оба числа заданы и одно больше другого)
                if (startNumber < finishNumber && startNumber > -1)
                    result = result.TakeWhile((e, index) =>
                        index + 1 >= startNumber && index + 1 < finishNumber
                    );
            }

            // Возвращаем данные после всех операций
            return new List<Invoice>(result);
        }

        /// <summary>
        /// Изменение счета по id
        /// invoiceNumber: идентификатор (номер счета), должен соответствовать идентификатору в data
        /// data - обновленные данные счета
        /// Если изменен, вернет запись, если не изменен, вернет null
        /// </summary>
        public Invoice EditInvoice(int invoiceNumber, Invoice data)
        {
            // Проверка на входящие параметры
            if (data is null)
                return null;

            // Id всегда должен соответствовать
            if (invoiceNumber != data.InvoiceNumber)
                return null;

            // Проверка на существование элемента
            var invoicesList = GetInvoices();
            var invoice = invoicesList.FirstOrDefault((e) => e.InvoiceNumber == invoiceNumber);
            if (invoice is null)
                return null;

            // Запись данных
            var result = DataWriter.EditInvoiceById(invoiceNumber, data);

            // Проверка успешной записи
            if (result)
                return data;

            return null;
        }

        /// <summary>
        /// Возврат счета по id
        /// invoiceNumber: идентификатор (номер счета)
        /// Если не существует, вернет null
        /// </summary>
        // В данном виде просто обертка над DataReader.GetInvoiceByIdFromCsvFile
        public Invoice GetInvoiceById(int InvoiceNumber)
        {
            return DataReader.GetInvoiceByIdFromCsvFile(InvoiceNumber);
        }

        /// <summary>
        /// Добавление счета (в файл)
        /// invoice - счет
        /// Если не удалось добавить, вернет null
        /// </summary>
        // У меня вот здесь сомнения, возможно имеет смысл сделать создание по передачи только баланса (мб и статусов)
        // оставлю так, но в теории хз как будет сделать правильнее (логичнее)
        public Invoice AddInvoice(Invoice invoice)
        {
            // Номер должен быть заполнен
            if (invoice.InvoiceNumber == -1)
                return null;

            // Сумма должна быть заполнена и быть числом
            if (invoice.Balance == null)
                return null;

            // Проверка на существование элемента (не должно существовать)
            var invoicesList = GetInvoices();
            var temp = invoicesList.FirstOrDefault((e) => e.InvoiceNumber == invoice.InvoiceNumber);
            if (!(invoice is null))
                return null;

            if (invoice.CreationDate == new DateTime())
                invoice.CreationDate = DateTime.Now;

            var functionResult = DataWriter.PushInvoiceToCsvFile(invoice);

            if (functionResult)
                return invoice;

            return null;
        }
    }
}
