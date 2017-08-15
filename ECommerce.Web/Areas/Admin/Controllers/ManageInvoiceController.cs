using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageInvoiceController : Controller
    {
        private ProductModel productModel = new ProductModel();
        private InvoiceModel invoiceModel = new InvoiceModel();
        public ActionResult Index()
        {
            var model = invoiceModel.Get();
            return View(model);
        }

        public ActionResult Delivered()
        {
            var model = invoiceModel.Get()
                .Where(x => x.Status == Core.CustomerInvoice.InvoiceStatus.Delivered).ToList();
            return View(model);
        }

        public ActionResult PlacedOrder()
        {
            var model = invoiceModel.Get()
                .Where(x => x.Status == Core.CustomerInvoice.InvoiceStatus.PlacedOrder).ToList();
            return View(model);
        }

        public ActionResult OnDelivery()
        {
            var model = invoiceModel.Get()
                .Where(x => x.Status == Core.CustomerInvoice.InvoiceStatus.OnDelivery).ToList();
            return View(model);
        }

        public ActionResult Details(Guid id)
        {
            try
            {
                var model = invoiceModel.GetDetails(id);
                return View(model);
            }
            catch
            {
                Session["Notify"] = "Failed to load invoide details.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Update(Guid id)
        {
            try
            { 
                var model = invoiceModel.GetDetails(id);
                if (model.Status == Core.CustomerInvoice.InvoiceStatus.Delivered)
                {
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch
            {
                Session["Notify"] = "Failed to load invoide details.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Update(InvoiceViewModel model)
        {
            invoiceModel.Update(model);
            return RedirectToAction("Index");
        }
    }
}