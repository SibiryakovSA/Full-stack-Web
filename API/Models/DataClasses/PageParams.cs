using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DataClasses
{
    // Параметры страницы
    public class PageParams
    {
        // Индекс записи от
        public int StartNumber { get; set; } = -1;

        // Индекс записи до (не включительно)
        public int FinishNumber { get; set; } = -1;

        // Параметры фильтрации
        public FilteringParams FilteringParams { get; set; } = null;
    }
}
