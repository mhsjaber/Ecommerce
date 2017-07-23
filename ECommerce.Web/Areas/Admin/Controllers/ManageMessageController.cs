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
    }
}