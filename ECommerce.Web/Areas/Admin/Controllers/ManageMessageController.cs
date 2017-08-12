using ECommerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
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
            }
            catch { }
            return RedirectToAction("Index");
        }
    }
}