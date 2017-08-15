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
            try
            {
                subCategoryModel.CreateSubCategory(model);
                Session["Notify"] = "Sub category added successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to add sub category.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Update(Guid id)
        {
            try
            {
                var model = subCategoryModel.GetDetails(id);
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
        public ActionResult Update(SubCategoryViewModel model)
        {
            try
            {
                subCategoryModel.UpdateSubCategory(model);
                Session["Notify"] = "Sub category updated successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to update sub category.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid SubCategoryID)
        {
            try
            {
                subCategoryModel.DeleteSubCategory(SubCategoryID);
                Session["Notify"] = "Sub category delete successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to delete sub category.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }
    }
}