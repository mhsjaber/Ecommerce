using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            var model = productModel.Get();
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
            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/img/product/" + filename));
                model.ImageFile.SaveAs(path);
                model.Image = filename;
            }
            productModel.Create(model);
            return RedirectToAction("Index");
        }

        public ActionResult Update(Guid id)
        {
            var model = productModel.GetDetails(id);
            model.Category = categoryModel.GetCategories();
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ProductViewModel model)
        {
            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/img/product/" + filename));
                model.ImageFile.SaveAs(path);
                model.Image = filename;
            }
            productModel.Update(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid ID)
        {
            productModel.Delete(ID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetSubCategory(Guid CategoryID)
        {
            var categories = subCategoryModel.GetSubCategories().Where(x => x.CategoryID == CategoryID).ToList();
            return Json(categories);
        }

        [HttpPost]
        public ActionResult StoreItem(ProductViewModel model)
        {
            productModel.StoreItem(model);
            return RedirectToAction("Index");
        }
    }
}