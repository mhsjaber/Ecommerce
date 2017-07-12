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
            var category = new ProductCategory();
            category.Name = model.CategoryName;
            _unit.ProductCategoryRepository.Add(category);
            _unit.Save();
        }

        public void UpdateCategory(CategoryViewModel model)
        {
            var category = _unit.ProductCategoryRepository.GetById(model.CategoryID);
            category.Name = model.CategoryName;
            _unit.ProductCategoryRepository.Update(category);
            _unit.Save();
        }

        public void DeleteCategory(Guid categoryID)
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
        }
    }
}