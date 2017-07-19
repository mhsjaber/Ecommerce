using ECommerce.Core;
using ECommerce.Core.CustomerInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class InvoiceViewModel
    {
        public DateTime CreatedOn { get; set; }
        public Guid? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public InvoiceStatus Status { get; set; }
        public List<InvoiceProductViewModel> Products { get; set; }
        public Guid InvoiceID { get; set; }
        public int Number { get; set; }
    }

    public class InvoiceProductViewModel
    {
        public Guid ID { get; set; }
        public Guid InvoiceID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Disccount { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }

    public class InvoiceModel
    {
        private ECommerceUnitOfWork _unit;
        public InvoiceModel()
        {
            _unit = new ECommerceUnitOfWork(new ECommerceContext());
        }

        public List<InvoiceViewModel> Get()
        {
            var invoices = _unit.InvoiceRepository.GetAll().OrderByDescending(x => x.CreatedOn).ToList();
            var list = new List<InvoiceViewModel>();
            foreach (var item in invoices)
            {
                list.Add(new InvoiceViewModel()
                {
                    InvoiceID = item.ID,
                    Status = item.Status,
                    CustomerID = item.CustomerID,
                    CustomerName = item.CustomerID.HasValue ? _unit.CustomerRepository.GetById(item.CustomerID.Value).FullName : "",
                    CreatedOn = item.CreatedOn,
                    Number = item.Number
                });
            }
            return list;
        }

        public InvoiceViewModel GetDetails(Guid id)
        {
            var invoice = _unit.InvoiceRepository.GetById(id);
            var dets = _unit.InvoiceProductRepository.GetAll().Where(x => x.InvoiceID == id).ToList();
            var model = new InvoiceViewModel();
            model.InvoiceID = id;
            model.CustomerName = invoice.CustomerID.HasValue ? _unit.CustomerRepository.GetById(invoice.CustomerID.Value).FullName : "";
            model.CreatedOn = invoice.CreatedOn;
            model.Number = invoice.Number;
            model.Status = invoice.Status;
            var prods = new List<InvoiceProductViewModel>();
            var prObj = new ProductModel();
            foreach (var item in dets)
            {
                prods.Add(new InvoiceProductViewModel() {
                    ProductName = prObj.GetDetails(item.ProductID).Name,
                    Quantity = item.Quantity,
                    Disccount = item.Disccount,
                    Price = item.Price,
                    Total = (item.Price * item.Quantity) - item.Disccount
                });
            }
            model.Products = prods;
            return model;
        }
    }
}