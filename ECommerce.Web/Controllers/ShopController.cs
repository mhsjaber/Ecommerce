using ECommerce.Core;
using ECommerce.Core.CustomerInvoice;
using ECommerce.Core.Member;
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

            if (invoice.Status != InvoiceStatus.InProgress)
                return RedirectToAction("Shipment");

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
                    Total = prod.Price * item.Quantity - item.Discount,
                    ID = item.ID
                };
                list.Add(p);
            }
            model.Products = list;
            model.GrandTotal = model.Products.Select(x => x.Total).ToList().Sum();
            return View(model);
        }

        public ActionResult Update(CartUpdateModel model, string submit)
        {
            try
            {
                var invoiceID = Guid.Parse(Session["InvoiceID"].ToString());
                var invoice = _unit.InvoiceRepository.GetById(invoiceID);
                if (submit != "Update")
                {
                    invoice.Status = InvoiceStatus.PlacedOrder;
                    _unit.InvoiceRepository.Update(invoice);
                }
                var products = _unit.InvoiceProductRepository.GetAll()
                        .Where(x => x.InvoiceID == invoiceID).ToList();
                foreach (var item in products)
                {
                    if (!model.ProductInvoiceID.Any(x => x == item.ID))
                    {
                        _unit.InvoiceProductRepository.Delete(item);
                    }
                }

                for (int i = 0; i < model.ProductInvoiceID.Length; i++)
                {
                    var product = products.Where(x => x.ID == model.ProductInvoiceID[i]).FirstOrDefault();
                    product.Quantity = model.Quantity[i];
                    _unit.InvoiceProductRepository.Update(product);
                }
                _unit.Save();
                Session["Notify"] = "Cart updated successfully.";
                Session["Type"] = "success";
            }
            catch (Exception ex)
            {
                Session["Notify"] = "Cart update failed. Error: " + ex.Message;
                Session["Type"] = "error";
            }
            if (submit == "Update")
                return RedirectToAction("Details");
            return RedirectToAction("Shipment");
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (model.Password != model.RePassword)
            {
                Session["Notify"] = "Password and Confirm Password not matching.";
                Session["Type"] = "error";
                return RedirectToAction("Shipment");
            }

            var user = _unit.CustomerRepository.GetAll()
                   .Where(x => x.Username.ToLower() == model.Username.ToLower())
                   .FirstOrDefault();

            if (user == null)
            {
                var customer = new Customer();
                customer.Address = model.Address;
                customer.Email = model.Email;
                customer.FullName = model.FullName;
                customer.Mobile = model.Mobile;
                customer.Password = model.Password;
                customer.Status = CustomerStatus.Temporary;
                customer.Username = model.Username;
                customer.CreatedOn = DateTime.Now;
                _unit.CustomerRepository.Add(customer);
                _unit.Save();
                Session["Username"] = model.Username;
                if (Session["InvoiceID"] != null)
                {
                    var invoice = _unit.InvoiceRepository.GetById(Guid.Parse(Session["InvoiceID"].ToString()));
                    invoice.CustomerID = customer.ID;
                    _unit.InvoiceRepository.Update(invoice);
                    _unit.Save();
                }
                Session["Notify"] = "User registered successfully.";
                Session["Type"] = "success";
            }
            else
            {
                Session["Notify"] = "Username already taken.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Shipment");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var user = _unit.CustomerRepository.GetAll()
                   .Where(x => x.Username == model.Username && x.Password == model.Password)
                   .FirstOrDefault();

            if (user != null)
            {
                Session["Username"] = model.Username;
                if (Session["InvoiceID"] != null)
                {
                    var invoice = _unit.InvoiceRepository.GetById(Guid.Parse(Session["InvoiceID"].ToString()));
                    invoice.CustomerID = user.ID;
                    _unit.InvoiceRepository.Update(invoice);
                    _unit.Save();
                }
            }
            else
            {
                Session["Notify"] = "Username and password not matching.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Shipment");
        }

        public ActionResult Shipment()
        {
            try
            {
                var invoiceID = Guid.Parse(Session["InvoiceID"].ToString());
                var invoice = _unit.InvoiceRepository.GetById(invoiceID);
                
                return View();
            }
            catch (Exception ex)
            {
                Session["Notify"] = "Failed to load shipment details.";
                Session["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Shipment(ShipAddressModel model)
        {
            try
            {
                var ship = new BillingAddress();
                ship.Address = model.Address;
                ship.Email = model.Email;
                ship.Name = model.FullName;
                ship.Mobile = model.Mobile;
                ship.OptionalMobile = model.OptionalMobile;
                _unit.BillingAddressRepository.Add(ship);

                var invoiceID = Guid.Parse(Session["InvoiceID"].ToString());
                var invoice = _unit.InvoiceRepository.GetById(invoiceID);
                invoice.BillingAddressID = ship.ID;
                _unit.InvoiceRepository.Update(invoice);
                _unit.Save();

                Session["Notify"] = "Order placed successfully.";
                Session["Type"] = "success";
                return RedirectToAction("Complete");
            }
            catch (Exception ex)
            {
                Session["Notify"] = "Failed to place order.";
                Session["Type"] = "error";
                return View();
            }
        }

        public ActionResult Complete()
        {
            if (Session["InvoiceID"] == null)
            {
                Session["Notify"] = "Order already placed or invoice not found.";
                Session["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }

            Session["InvoiceID"] = null;
            return View();
        }
    }
}