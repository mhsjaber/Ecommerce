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
        public HttpPostedFileBase ImageFile { get; set; }
        public List<ProductSubCategory> SubCategory { get; set; }
    }

    public class ProductModel
    {
        private ECommerceUnitOfWork _unit;
        public ProductModel()
        {
            _unit = new ECommerceUnitOfWork(new ECommerceContext());
        }

        public List<ProductViewModel> Get()
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

        public ProductViewModel GetDetails(Guid id)
        {
            var product = _unit.ProductRepository.GetById(id);
            var model = new ProductViewModel();
            model.SubCategoryID = product.SubCategoryID;
            model.ID = product.ID;
            model.Color = product.Color;
            model.Description = product.Description;
            model.Image = product.Image;
            model.Name = product.Name;
            model.Price = product.Price;
            model.CategoryID = _unit.ProductSubCategoryRepository.GetById(product.SubCategoryID).CategoryID;
            model.SubCategory = _unit.ProductSubCategoryRepository.GetAll().ToList().Where(x => x.CategoryID == model.CategoryID).ToList();
            return model;
        }

        public void StoreItem(ProductViewModel model)
        {
            var product = _unit.ProductRepository.GetById(model.ID);
            product.UnitAvailable += model.UnitAvailable;
            _unit.ProductRepository.Update(product);
            _unit.Save();
        }

        public void Create(ProductViewModel model)
        {
            var product = new Product();
            product.Description = model.Description;
            product.Color = model.Color;
            product.Name = model.Name;
            product.Price = model.Price;
            product.Image = model.Image;
            product.SubCategoryID = model.SubCategoryID;
            _unit.ProductRepository.Add(product);
            _unit.Save();
        }

        public void Update(ProductViewModel model)
        {
            var product = _unit.ProductRepository.GetById(model.ID);
            product.Description = model.Description;
            product.Color = model.Color;
            product.Name = model.Name;
            product.Price = model.Price;
            product.Image = model.Image;
            product.SubCategoryID = model.SubCategoryID;
            _unit.ProductRepository.Update(product);
            _unit.Save();
        }

        public void Delete(Guid productID)
        {
            var product = _unit.ProductRepository.GetById(productID);
            _unit.ProductRepository.Delete(product);
            _unit.Save();
        }
    }
}