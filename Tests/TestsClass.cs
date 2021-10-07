using API.Models;
using API.Models.DataClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class TestsClass
    {
        public TestsClass()
        {
            //  остыль, запускаетс€ не через дебаг
            Environment.SetEnvironmentVariable("DataFilePath", "C:\\Users\\User\\OneDrive\\Desktop\\Full-stack Web\\API\\Invoices.csv");
        }

        [TestMethod]
        public void TestFileExist()
        {
            var path = Environment.GetEnvironmentVariable("DataFilePath");
            var b = File.Exists(path);
            Console.WriteLine("FilePath: " + path);

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestDataReaderGetAll()
        {
            var invoices = (List<Invoice>)DataReader.GetAllInvoicesFromCsvFile();
            Console.WriteLine(invoices.Count);
            Assert.IsTrue(invoices.Count >= 0);
        }

        [TestMethod]
        public void TestReadOneInvoice()
        {
            var invoice = DataReader.GetInvoiceByIdFromCsvFile(5);
            Console.WriteLine(invoice.ToString());
            Assert.IsTrue(invoice.InvoiceNumber == 5);
        }

        [TestMethod]
        public void TestWriteOneInvoice()
        {
            var invoices = (List<Invoice>)DataReader.GetAllInvoicesFromCsvFile();


            var tempInvoice = new Invoice()
            {
                CreationDate = DateTime.Now,
                EditionDate = new DateTime(),
                InvoiceNumber = invoices.Count + 1,
                ProcessingStatus = ProcessingStatus.New,
                Balance = 0,
                PaymentMethod = PaymentMethod.Undefind
            };
            Console.WriteLine(tempInvoice.ToString());

            var b = DataWriter.PushInvoiceToCsvFile(tempInvoice);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestEditOneInvoice()
        {
            var tempInvoice = new Invoice()
            {
                CreationDate = DateTime.Now,
                EditionDate = new DateTime(),
                InvoiceNumber = 10,
                ProcessingStatus = ProcessingStatus.New,
                Balance = 333.333,
                PaymentMethod = PaymentMethod.Undefind
            };
            Console.WriteLine(tempInvoice.ToString());

            var b = DataWriter.EditInvoiceById(10, tempInvoice);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestGetInvoicesFormManagerWithoutParams()
        {
            var dm = new DataManager();
            var res = dm.GetInvoices();
            Console.WriteLine(res.Count());

            Assert.IsTrue(res != null);
        }

        [TestMethod]
        public void TestGetInvoicesFormManager()
        {
            var dm = new DataManager();

            // ƒолжно быть 2 записи
            var fp = new FilteringParams()
            {
                InvoiceNumberMax = 4,
                InvoiceNumberMin = 1,
                processingStatus = ProcessingStatus.Cancelled
            };

            //должна остатьс€ 1 запись
            var pp = new PageParams()
            {
                StartNumber = 1,
                FinishNumber = 2
            };

            var res = dm.GetInvoices(null, fp, pp);

            Assert.IsTrue(res.Count == 1 && res[0].InvoiceNumber == 1);
        }

        [TestMethod]
        public void TestEditInvoiceFormManagerCorrect()
        {
            var dm = new DataManager();
            var invoices = dm.GetInvoices();
            

            var tempInvoice = new Invoice()
            {
                CreationDate = DateTime.Now,
                EditionDate = new DateTime(),
                InvoiceNumber = invoices.Count,
                ProcessingStatus = ProcessingStatus.New,
                Balance = -500,
                PaymentMethod = PaymentMethod.Undefind
            };
            Console.WriteLine(tempInvoice.ToString());

            var res = dm.EditInvoice(invoices.Count, tempInvoice);
            Assert.IsTrue(res.InvoiceNumber == invoices.Count);
        }

        [TestMethod]
        public void TestEditInvoiceFormManagerIncorrect()
        {
            var dm = new DataManager();
            var invoices = dm.GetInvoices();

            var tempInvoice = new Invoice()
            {
                CreationDate = DateTime.Now,
                EditionDate = new DateTime(),
                InvoiceNumber = invoices.Count - 1,
                ProcessingStatus = ProcessingStatus.New,
                Balance = -500,
                PaymentMethod = PaymentMethod.Undefind
            };
            Console.WriteLine(tempInvoice.ToString());

            var res1 = dm.EditInvoice(invoices.Count, null);
            var res2 = dm.EditInvoice(invoices.Count, tempInvoice);
            Assert.IsTrue(res1 == null && res2 == null);
        }


    }
}
