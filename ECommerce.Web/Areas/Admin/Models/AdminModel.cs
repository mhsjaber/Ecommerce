using ECommerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class AdminViewModel
    {
        public List<Core.Member.Admin> Admins { get; set; }
        public bool CanEdit { get; set; }
    }

    public class AdminLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminModel
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public AdminViewModel GetAdmins()
        {
            var model = new AdminViewModel();
            model.Admins = _unit.AdminRepository.GetAll().ToList();
            return model;
        }

    }
}