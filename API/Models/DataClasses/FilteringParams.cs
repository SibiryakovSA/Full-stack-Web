using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DataClasses
{
    // Параметры фильтрации для запроса
    public class FilteringParams
    {
        // Диапазон значений номера счета
        public int InvoiceNumberMin { get; set; } = -1;
        public int InvoiceNumberMax { get; set; } = -1;

        // Статус обработки
        public ProcessingStatus processingStatus { get; set; } = ProcessingStatus.Undefind;

        // Способ оплаты
        public PaymentMethod paymentMethod { get; set; } = PaymentMethod.Undefind;
    }
}
