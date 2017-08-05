using ECommerce.Core;
using ECommerce.Core.Member;
using ECommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public ActionResult Index()
        {
            var model = new HomeModel();
            model.Products = _unit.ProductRepository.GetAll().ToList();

            var featured = _unit.FeaturedProductRepository.GetAll().ToList().Select(x=> x.ProductID).ToList();
            var fs = model.Products.Where(x => featured.Any(a => a == x.ID)).ToList();
            model.FeaturedProducts = fs;

            return View(model);
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

        [HttpPost]
        public ActionResult Contact(Message model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.FullName) && !string.IsNullOrWhiteSpace(model.MessageBody) && !string.IsNullOrWhiteSpace(model.Subject))
            {
                model.CreatedOn = DateTime.Now;
                _unit.MessageRepository.Add(model);
                _unit.Save();
                Session["Notify"] = "Message sent successfully";
                Session["Type"] = "success";
            }
            else
            {
                Session["Notify"] = "Please fill up all required fields.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Contact");
        }
    }
}