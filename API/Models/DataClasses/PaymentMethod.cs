using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DataClasses
{
    // Способ оплаты счета
    public enum PaymentMethod
    {
        // Не задан
        Undefind = -1,
        // Кредитная карта
        CreditCard = 1,
        // Дебетовая карта
        DebitCard = 2,
        // Электронный чек
        ElectronicCheck = 3
    }
}
