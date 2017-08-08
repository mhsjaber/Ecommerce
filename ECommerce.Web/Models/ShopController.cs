using ECommerce.Core;
using ECommerce.Core.CustomerInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Models
{
    public class ShopController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        [HttpPost]
        public ActionResult AddToCart(Guid id)
        {
            try
            {
                if (Session["InvoiceID"] == null)
                {
                    var invoice = new Invoice();
                    invoice.CreatedOn = DateTime.Now;
                    if (Session["Username"] != null)
                        invoice.CustomerID = _unit.CustomerRepository.GetAll()
                            .Where(x => x.Username == Session["Username"].ToString()).FirstOrDefault().ID;
                    invoice.Status = InvoiceStatus.InProgress;
                    _unit.InvoiceRepository.Add(invoice);
                    Session["InvoiceID"] = invoice.ID;

                    var product = _unit.ProductRepository.GetById(id);
                    var invProduct = new InvoiceProduct();

                    invProduct.Quantity = 1;
                    invProduct.ProductID = id;
                    invProduct.Price = product.Price;
                    invProduct.Discount = 0;
                    invProduct.InvoiceID = invoice.ID;

                    _unit.InvoiceProductRepository.Add(invProduct);
                    _unit.Save();

                }
                else
                {
                    var product = _unit.ProductRepository.GetById(id);
                    var available = _unit.InvoiceProductRepository.GetAll().Where(x => x.ProductID == id && x.InvoiceID == Guid.Parse(Session["InvoiceID"].ToString())).FirstOrDefault();

                    if (available == null)
                    {
                        var invProduct = new InvoiceProduct();
                        invProduct.Quantity = 1;
                        invProduct.ProductID = id;
                        invProduct.Price = product.Price;
                        invProduct.Discount = 0;
                        invProduct.InvoiceID = Guid.Parse(Session["InvoiceID"].ToString());
                        _unit.InvoiceProductRepository.Add(invProduct);
                    }
                    else
                    {
                        available.Quantity++;
                        _unit.InvoiceProductRepository.Update(available);
                    }
                    _unit.Save();
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
    }
}