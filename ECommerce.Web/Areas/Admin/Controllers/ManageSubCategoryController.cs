using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageSubCategoryController : Controller
    {
        private SubCategoryModel subCategoryModel = new SubCategoryModel();
        private CategoryModel categoryModel = new CategoryModel();
        public ActionResult Index()
        {
            var model = subCategoryModel.GetSubCategories();
            return View(model);
        }

        public ActionResult Create()
        {
            try
            {
                var model = new SubCategoryViewModel();
                model.Category = categoryModel.GetCategories();
                return View(model);
            }
            catch
            {
                Session["Notify"] = "Failed to load sub category.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Create(SubCategoryViewModel model)
        {
            subCategoryModel.CreateSubCategory(model);
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
    }
}