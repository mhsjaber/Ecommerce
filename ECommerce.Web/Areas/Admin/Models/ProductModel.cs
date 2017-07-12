using ECommerce.Core;
using ECommerce.Core.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public List<CategoryViewModel> Category { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Guid SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int ItemNumber { get; set; }
        public int Price { get; set; }
        public int UnitAvailable { get; set; }
        public string Image { get; set; }
        public HttpPostedFile ImageFile { get; set; }
    }

    public class ProductModel
    {
        private ECommerceUnitOfWork _unit;
        public ProductModel()
        {
            _unit = new ECommerceUnitOfWork(new ECommerceContext());
        }

        public List<ProductViewModel> GetProducts()
        {
            var products = _unit.ProductRepository.GetAll();
            var list = new List<ProductViewModel>();

            foreach (var product in products)
            {
                var smodel = new SubCategoryModel();
                var subCategory = smodel.GetDetails(product.SubCategoryID);
                var obj = new ProductViewModel();
                obj.Name = product.Name;
                obj.SubCategoryName = subCategory.SubCategoryName;
                obj.CategoryName = subCategory.CategoryName;
                obj.ItemNumber = product.ItemNumber;
                obj.UnitAvailable = product.UnitAvailable;
                obj.ID = product.ID;
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

        public void CreateProduct(ProductViewModel model)
        {
            var product = new Product();
            product.Description = model.Description;
            product.Color = model.Color;
            product.Name = model.Name;
            product.Price = model.Price;
            product.SubCategoryID = model.SubCategoryID;
            _unit.ProductRepository.Add(product);
            _unit.Save();
        }

        public void UpdateSubCategory(SubCategoryViewModel model)
        {
            var subCategory = _unit.ProductSubCategoryRepository.GetById(model.CategoryID);
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