using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DataClasses
{
    // Статус обработки счета
    public enum ProcessingStatus
    {
        // Не задан
        Undefind = -1, 
        // Новый
        New = 1,
        // Оплаченный
        Paid = 2,
        // Отмененный
        Cancelled = 3
    }
}
