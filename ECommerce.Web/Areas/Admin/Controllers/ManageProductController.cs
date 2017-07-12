using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    public class ManageProductController : Controller
    {
        private ProductModel productModel = new ProductModel();
        private CategoryModel categoryModel = new CategoryModel();
        private SubCategoryModel subCategoryModel = new SubCategoryModel();
        public ActionResult Index()
        {
            var model = productModel.GetProducts();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ProductViewModel();
            model.Category = categoryModel.GetCategories();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel model)
        {
            productModel.CreateProduct(model);
            return RedirectToAction("Index");
        }

        public ActionResult Update(Guid id)
        {
            var model = subCategoryModel.GetDetails(id);
            model.Category = categoryModel.GetCategories();
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(SubCategoryViewModel model)
        {
            subCategoryModel.UpdateSubCategory(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid SubCategoryID)
        {
            subCategoryModel.DeleteSubCategory(SubCategoryID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetSubCategory(Guid CategoryID)
        {
            var categories = subCategoryModel.GetSubCategories().Where(x => x.CategoryID == CategoryID).ToList();
            return Json(categories);
        }
    }
}