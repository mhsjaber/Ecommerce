using ECommerce.Core;
using ECommerce.Core.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var unit = new ECommerceUnitOfWork(new ECommerceContext());
            var customer = new Customer();
            customer.FullName = "Jaber Kibria";
            unit.CustomerRepository.Add(customer);
            unit.Save();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}