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
        public string Mobile { get; set; }
        public string Address { get; set; }
        public InvoiceStatus Status { get; set; }
        public List<InvoiceProductViewModel> Products { get; set; }
        public Guid InvoiceID { get; set; }
        public int Number { get; set; }

        public int[] ProductQuantity { get; set; }
        public double[] ProductDiscount { get; set; }
        public Guid[] InvoiceProductID { get; set; }
        public BillingAddress BillingAddress { get; set; }
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
        public int UnitAvailable { get; internal set; }
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

        public void Update(InvoiceViewModel model)
        {
            try
            {
                var invoice = _unit.InvoiceRepository.GetById(model.InvoiceID);
                var prods = _unit.InvoiceProductRepository.GetAll().ToList().Where(x => x.InvoiceID == model.InvoiceID).ToList();
                foreach (var item in prods)
                {
                    var p = _unit.ProductRepository.GetById(item.ProductID);
                    if (invoice.Status == InvoiceStatus.OnDelivery)
                    {
                        p.UnitAvailable += item.Quantity;
                    }
                    if (model.InvoiceProductID != null && model.InvoiceProductID.Count() > 0 && model.InvoiceProductID.Any(x => x == item.ID))
                    {
                        var index = Array.IndexOf(model.InvoiceProductID, item.ID);
                        if (model.Status == InvoiceStatus.Delivered || model.Status == InvoiceStatus.OnDelivery)
                        {
                            p.UnitAvailable -= model.ProductQuantity[index];
                        }
                        item.Quantity = model.ProductQuantity[index];
                        item.Discount = model.ProductDiscount[index];
                        _unit.InvoiceProductRepository.Update(item);
                    }
                    else
                    {
                        _unit.InvoiceProductRepository.Delete(item);
                    }
                }
                invoice.Status = model.Status;
                _unit.InvoiceRepository.Update(invoice);
                _unit.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public InvoiceViewModel GetDetails(Guid id)
        {
            try
            {
                var invoice = _unit.InvoiceRepository.GetById(id);
                var dets = _unit.InvoiceProductRepository.GetAll().Where(x => x.InvoiceID == id).ToList();
                var model = new InvoiceViewModel();
                model.InvoiceID = id;
                if (invoice.CustomerID.HasValue)
                {
                    var customer = _unit.CustomerRepository.GetById(invoice.CustomerID.Value);
                    model.CustomerName = customer.FullName;
                    model.Mobile = customer.Mobile;
                    model.Address = customer.Address;
                }
                model.CreatedOn = invoice.CreatedOn;
                model.Number = invoice.Number;
                model.Status = invoice.Status;
                if (invoice.BillingAddressID.HasValue)
                    model.BillingAddress = _unit.BillingAddressRepository.GetById(invoice.BillingAddressID.Value);
                var prods = new List<InvoiceProductViewModel>();
                var prObj = new ProductModel();
                foreach (var item in dets)
                {
                    int x = 0;
                    if (model.Status == InvoiceStatus.OnDelivery)
                    {
                        x = item.Quantity;
                    }
                    var p = _unit.ProductRepository.GetById(item.ProductID);
                    prods.Add(new InvoiceProductViewModel()
                    {
                        ProductName = prObj.GetDetails(item.ProductID).Name,
                        Quantity = item.Quantity,
                        UnitAvailable = p.UnitAvailable + x,
                        Disccount = item.Discount,
                        Price = item.Price,
                        Total = (item.Price * item.Quantity) - item.Discount,
                        ID = item.ID
                    });
                }

                model.Products = prods;
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}