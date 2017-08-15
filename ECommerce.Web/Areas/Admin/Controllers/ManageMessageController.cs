using ECommerce.Core;
using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageMessageController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public ActionResult Index()
        {
            var model = _unit.MessageRepository.GetAll();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var msg = _unit.MessageRepository.GetById(id);
                _unit.MessageRepository.Delete(msg);
                _unit.Save();
                Session["Notify"] = "Message deleted successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to delete message";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }
    }
}