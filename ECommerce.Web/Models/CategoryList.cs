using ECommerce.Core;
using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{

    public class CategoryList
    {
        private static ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public static List<CategoryViewModel> GetAllCategory()
        {
            var model = new CategoryModel();

            var retList = new List<CategoryViewModel>();
            var list = model.GetCategories();

            foreach (var item in list)
            {
                var submodel = new SubCategoryModel();
                var subs = submodel.GetSubCategories().Where(x=> x.CategoryID == item.CategoryID).ToList();

                retList.Add(new CategoryViewModel()
                {
                    CategoryID = item.CategoryID,
                    CategoryName = item.CategoryName,
                    SubCategory = subs,
                    TotalProducts = _unit.ProductRepository.GetAll().Where(x=> subs.Any(y=> y.SubCategoryID == x.SubCategoryID)).ToList().Count()
                });
            }
            return retList;
        }
    }
}