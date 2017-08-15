using ECommerce.Core;
using ECommerce.Core.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategoryViewModel> SubCategory { get; set; }
        public int TotalProducts { get; internal set; }
    }

    public class CategoryModel
    {
        private ECommerceUnitOfWork _unit;
        public CategoryModel()
        {
            _unit = new ECommerceUnitOfWork(new ECommerceContext());
        }

        public List<CategoryViewModel> GetCategories()
        {
            var categories = _unit.ProductCategoryRepository.GetAll();
            var list = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var obj = new CategoryViewModel();
                obj.CategoryID = category.ID;
                obj.CategoryName = category.Name;
                list.Add(obj);
            }
            return list;
        }

        public CategoryViewModel GetDetails(Guid id)
        {
            var category = _unit.ProductCategoryRepository.GetById(id);
            var model = new CategoryViewModel();
            model.CategoryID = category.ID;
            model.CategoryName = category.Name;
            return model;
        }

        public void CreateCategory(CategoryViewModel model)
        {
            try
            {
                var category = new ProductCategory();
                category.Name = model.CategoryName;
                _unit.ProductCategoryRepository.Add(category);
                _unit.Save();
                HttpContext.Current.Session["Notify"] = "Category created successfully.";
                HttpContext.Current.Session["Type"] = "success";
            }
            catch
            {
                HttpContext.Current.Session["Notify"] = "Error in category create.";
                HttpContext.Current.Session["Type"] = "error";
            }
        }

        public void UpdateCategory(CategoryViewModel model)
        {
            try
            {
                var category = _unit.ProductCategoryRepository.GetById(model.CategoryID);
                category.Name = model.CategoryName;
                _unit.ProductCategoryRepository.Update(category);
                _unit.Save();
                HttpContext.Current.Session["Notify"] = "Category updated successfully.";
                HttpContext.Current.Session["Type"] = "success";
            }
            catch
            {
                HttpContext.Current.Session["Notify"] = "Error in category update.";
                HttpContext.Current.Session["Type"] = "error";
            }
        }

        public void DeleteCategory(Guid categoryID)
        {
            try
            {
                var category = _unit.ProductCategoryRepository.GetById(categoryID);
                var subCategories = _unit.ProductSubCategoryRepository.GetAll().Where(x => x.CategoryID == categoryID).ToList();
                foreach (var sub in subCategories)
                {
                    _unit.ProductSubCategoryRepository.Delete(sub);
                    _unit.Save();
                }
                _unit.ProductCategoryRepository.Delete(category);
                _unit.Save();
                HttpContext.Current.Session["Notify"] = "Category deleted successfully.";
                HttpContext.Current.Session["Type"] = "success";
            }
            catch
            {
                HttpContext.Current.Session["Notify"] = "Error in category delete.";
                HttpContext.Current.Session["Type"] = "error";
            }
        }
    }
}