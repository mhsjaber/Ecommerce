using ECommerce.Core;
using ECommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public ActionResult Index(int index = 1)
        {
            var all = _unit.ProductRepository.GetAll().ToList();
            var model = new ProductModel();
            model.TotalPages = (int)Math.Ceiling((double)all.Count / 6);
            if (index > model.TotalPages)
                index = model.TotalPages;
            else if (index < 1)
                index = 1;
            model.Products = all.Skip((index - 1) * 6).Take(6).ToList();
            model.Index = index;
            return View(model);
        }

        public ActionResult Details(Guid id)
        {
            var model = new ProductDetailsModel();
            model.Product = _unit.ProductRepository.GetById(id);
            model.MoreProducts = _unit.ProductRepository.GetAll().ToList();
            return View(model);
        }
    }
}