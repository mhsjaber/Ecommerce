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
        public List<InvoiceProduct> Products { get; set; }
        public Guid InvoiceID { get; set; }
        public int Number { get; set; }
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

    }
}