using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageCategoryController : Controller
    {
        private CategoryModel categoryModel = new CategoryModel();
        public ActionResult Index()
        {
            var model = categoryModel.GetCategories();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            categoryModel.CreateCategory(model);
            return RedirectToAction("Index");
        }

        public ActionResult Update(Guid id)
        {
            var model = categoryModel.GetDetails(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(CategoryViewModel model)
        {
            categoryModel.UpdateCategory(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid CategoryID)
        {
            categoryModel.DeleteCategory(CategoryID);
            return RedirectToAction("Index");
        }
    }
}