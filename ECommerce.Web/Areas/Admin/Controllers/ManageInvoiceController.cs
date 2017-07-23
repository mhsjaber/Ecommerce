using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
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

        public ActionResult Details(Guid id)
        {
            var model = invoiceModel.GetDetails(id);
            return View(model);
        }

        public ActionResult Update(Guid id)
        {
            var model = invoiceModel.GetDetails(id);
            return View(model);
        }

    }
}