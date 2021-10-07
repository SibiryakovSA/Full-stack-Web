using API.Models;
using API.Models.DataClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Invoices")]
    [ApiController]
    public class InvoicesController : Controller
    {
        DataManager _dm = new DataManager();
        public InvoicesController()
        {

        }

        // Получение счета по идентификатору
        [HttpGet("{id}")]
        public IActionResult GetInvoiceById(int id)
        {
            var res = _dm.GetInvoiceById(id);

            if (!(res is null))
                return Ok(res);

            return NotFound("Счет с таким идентификатором не найден");
        }

        // Дефолтный вывод (без параметров)
        [HttpGet("")]
        public IActionResult GetInvoices()
        {
            var invoices = _dm.GetInvoices();
            if (invoices.Count > 0)
                return Ok(invoices);

            return NotFound("В системе нет счетов");
        }

        // Получение списка всех счетов
        [HttpGet("WithParams")]
        public IActionResult GetInvoices([FromBody]PageParams pageParams)
        {
            var invoices = _dm.GetInvoices(null, pageParams);
            if (invoices.Count > 0)
                return Ok(invoices);

            return NotFound("Счета с выбранными условиями не найдены");
        }

        // Получение списка всех счетов (с сортировкой по идентификатору)
        [HttpGet("WithParams/Order/{order}")]
        public IActionResult GetInvoices(string order, [FromBody]PageParams pageParams)
        {
            bool? orderAsc = null;
            if (order.ToLower() == "asc")
                orderAsc = true;
            else if (order.ToLower() == "desc")
                orderAsc = false;
            else
                return BadRequest("Не удалось считать значение порядка сортировки");


            var invoices = _dm.GetInvoices(orderAsc, pageParams);
            if (invoices.Count > 0)
                return Ok(invoices);

            return NotFound("Счета с выбранными условиями не найдены");
        }

        // Добавление счета
        [HttpPost("Add")]
        public IActionResult AddInvoice([FromBody]Invoice invoice)
        {
            var res = _dm.AddInvoice(invoice);

            if (!(res is null))
                return Ok(res);

            return BadRequest("Счет не удалось добавить, переданы некорректные данные");
        }

        // Изменение счета
        [HttpPut("Edit/{id}")]
        public IActionResult EditInvoice(int id, [FromBody]Invoice invoice)
        {
            var res = _dm.EditInvoice(id, invoice);

            if (!(res is null))
                return Ok(res);

            return BadRequest("Счет не удалось изменить, переданы некорректные данные");
        }
    }
}
