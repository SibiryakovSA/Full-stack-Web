using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DataClasses
{
    public class Invoice
    {
        // Дата создания счета
        public DateTime CreationDate { get; set; } = new DateTime();
        // Дата изменения счета (может быть пустой!)
        public DateTime EditionDate { get; set; } = new DateTime();
        // Номер счета
        public int InvoiceNumber { get; set; } = -1;
        // Статус обработки счета
        public ProcessingStatus ProcessingStatus { get; set; } = ProcessingStatus.Undefind;
        // Баланс счета (сумма счета)
        public double? Balance { get; set; } = null;
        // Способ оплаты счета
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Undefind;

        public void CopyTo(Invoice obj)
        {
            obj.CreationDate = CreationDate;
            obj.EditionDate = EditionDate;
            obj.InvoiceNumber = InvoiceNumber;
            obj.ProcessingStatus = ProcessingStatus;
            obj.Balance = Balance;
            obj.PaymentMethod = PaymentMethod;
        }
    }
}
