using ECommerce.Core;
using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AdminLoginModel model)
        {
            var user = _unit.AdminRepository
                .GetAll()
                .ToList()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefault();
            if (user != null)
            {
                Session["Admin"] = model.Username;
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
    }
}