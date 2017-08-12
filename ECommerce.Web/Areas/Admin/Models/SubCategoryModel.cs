using ECommerce.Core;
using ECommerce.Core.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class SubCategoryViewModel
    {
        public List<CategoryViewModel> Category { get; set; }
        public Guid SubCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
    }

    public class SubCategoryModel
    {
        private ECommerceUnitOfWork _unit;
        public SubCategoryModel()
        {
            _unit = new ECommerceUnitOfWork(new ECommerceContext());
        }

        public List<SubCategoryViewModel> GetSubCategories()
        {
            var subCategories = _unit.ProductSubCategoryRepository.GetAll();
            var list = new List<SubCategoryViewModel>();

            foreach (var subCategory in subCategories)
            {
                var model = new CategoryModel();
                var category = model.GetDetails(subCategory.CategoryID);
                var obj = new SubCategoryViewModel();
                obj.SubCategoryID = subCategory.ID;
                obj.SubCategoryName = subCategory.Name;
                obj.CategoryName = category.CategoryName;
                obj.CategoryID = subCategory.CategoryID;
                list.Add(obj);
            }
            return list;
        }

        public SubCategoryViewModel GetDetails(Guid id)
        {
            var subCategory = _unit.ProductSubCategoryRepository.GetById(id);
            var category = _unit.ProductCategoryRepository.GetById(subCategory.CategoryID);
            var model = new SubCategoryViewModel();
            model.SubCategoryID = subCategory.ID;
            model.SubCategoryName = subCategory.Name;
            model.CategoryID = category.ID;
            model.CategoryName = category.Name;
            return model;
        }

        public void CreateSubCategory(SubCategoryViewModel model)
        {
            var subCategory = new ProductSubCategory();
            subCategory.Name = model.SubCategoryName;
            subCategory.CategoryID = model.CategoryID;
            _unit.ProductSubCategoryRepository.Add(subCategory);
            _unit.Save();
        }

        public void UpdateSubCategory(SubCategoryViewModel model)
        {
            var subCategory = _unit.ProductSubCategoryRepository.GetById(model.SubCategoryID);
            subCategory.Name = model.SubCategoryName;
            subCategory.CategoryID = model.CategoryID;
            _unit.ProductSubCategoryRepository.Update(subCategory);
            _unit.Save();
        }

        public void DeleteSubCategory(Guid subCategoryID)
        {
            var subCategory = _unit.ProductSubCategoryRepository.GetById(subCategoryID);
            _unit.ProductSubCategoryRepository.Delete(subCategory);
            _unit.Save();
        }
    }
}