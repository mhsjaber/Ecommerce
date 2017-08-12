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
        public ActionResult Index(Guid? sub, Guid? cat, int size = 12, int index = 1)
        {
            var model = new ProductModel();
            var all = _unit.ProductRepository.GetAll().ToList();
            if (sub.HasValue)
                all = all.Where(x => x.SubCategoryID == sub.Value).ToList();
            if (cat.HasValue)
            {
                var subs = _unit.ProductSubCategoryRepository.GetAll().Where(x => x.CategoryID == cat.Value).ToList().Select(x => x.ID).ToList();
                all = all.Where(x => subs.Any( y=> y == x.SubCategoryID)).ToList();
            }
            model.TotalPages = (int)Math.Ceiling((double)all.Count / size);
            if (index > model.TotalPages)
                index = model.TotalPages;
            else if (index < 1)
                index = 1;
            model.Products = all.Skip((index - 1) * size).Take(size).ToList();
            model.SubCategory = sub;
            model.PageSize = size;
            ViewBag.Size = size;
            model.Category = cat;
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