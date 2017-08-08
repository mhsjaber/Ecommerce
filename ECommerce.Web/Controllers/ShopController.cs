using ECommerce.Core;
using ECommerce.Core.CustomerInvoice;
using ECommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ShopController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        [HttpPost]
        public ActionResult AddToCart(Guid id, int count)
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

                    invProduct.Quantity = count;
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
                        invProduct.Quantity = count;
                        invProduct.ProductID = id;
                        invProduct.Price = product.Price;
                        invProduct.Discount = 0;
                        invProduct.InvoiceID = Guid.Parse(Session["InvoiceID"].ToString());
                        _unit.InvoiceProductRepository.Add(invProduct);
                    }
                    else
                    {
                        available.Quantity += count;
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

        public ActionResult Details()
        {
            if (Session["InvoiceID"] == null)
            {
                Session["Notify"] = "Invoice not found.";
                Session["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }

            var invoiceID = Guid.Parse(Session["InvoiceID"].ToString());
            var invoice = _unit.InvoiceRepository.GetById(invoiceID);
            var products = _unit.InvoiceProductRepository.GetAll()
                .Where(x => x.InvoiceID == invoiceID).ToList();
            var model = new ShopModel();
            var list = new List<ProductList>();

            foreach (var item in products)
            {
                var prod = _unit.ProductRepository.GetById(item.ProductID);
                var p = new ProductList()
                {
                    Discount = item.Discount,
                    ImageLink = prod.Image,
                    Name = prod.Name,
                    Number = prod.ItemNumber.ToString("000000"),
                    Price = prod.Price,
                    Quantity = item.Quantity,
                    Total = prod.Price * item.Quantity - item.Discount
                };
                list.Add(p);
            }
            model.Products = list;
            model.GrandTotal = model.Products.Select(x => x.Total).ToList().Sum();
            return View(model);
        }
    }
}