using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageProductController : Controller
    {
        private ProductModel productModel = new ProductModel();
        private CategoryModel categoryModel = new CategoryModel();
        private SubCategoryModel subCategoryModel = new SubCategoryModel();
        public ActionResult Index()
        {
           List<ProductViewModel> model = productModel.Get();
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
            try
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/img/product/" + filename));
                    model.ImageFile.SaveAs(path);
                    model.Image = filename;
                }
                productModel.Create(model);
                Session["Notify"] = "Product created successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to create product.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Update(Guid id)
        {
            try
            {
                var model = productModel.GetDetails(id);
                model.Category = categoryModel.GetCategories();
                return View(model);
            }
            catch
            {
                Session["Notify"] = "Failed to load product.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Update(ProductViewModel model)
        {
            try
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/img/product/" + filename));
                    model.ImageFile.SaveAs(path);
                    model.Image = filename;
                }
                productModel.Update(model);
                Session["Notify"] = "Product updated successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to update product.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid ID)
        {
            try
            {
                productModel.Delete(ID);
                Session["Notify"] = "Product deleted successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to delete product.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetSubCategory(Guid CategoryID)
        {
            var categories = subCategoryModel.GetSubCategories().Where(x => x.CategoryID == CategoryID).ToList().OrderBy(x => x.SubCategoryName).ToList();
            return Json(categories);
        }

        [HttpPost]
        public JsonResult GetProducts(Guid SubCategoryID)
        {
            var prods = productModel.Get().Where(x => x.SubCategoryID == SubCategoryID).ToList().OrderBy(x => x.Name).ToList();
            return Json(prods);
        }

        [HttpPost]
        public ActionResult StoreItem(ProductViewModel model)
        {
            try
            {
                productModel.StoreItem(model);
                Session["Notify"] = "Product stored successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to store product.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }
    }
}