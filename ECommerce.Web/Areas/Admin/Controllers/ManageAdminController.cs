using ECommerce.Core;
using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    public class ManageAdminController : Controller
    {
        AdminModel adminModel = new AdminModel();
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public ActionResult Index()
        {
            var model = adminModel.GetAdmins();
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            var model = _unit.AdminRepository.GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Core.Member.Admin model)
        {
            var admin = _unit.AdminRepository.GetById(model.ID);
            admin.Name = model.Name;
            admin.Password = model.Password;
            admin.Type = model.Type;
            admin.Username = model.Username;
            _unit.AdminRepository.Update(admin);
            _unit.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Create(Guid id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Core.Member.Admin model)
        {
            var admin = new Core.Member.Admin();
            admin.Name = model.Name;
            admin.Password = model.Password;
            admin.Type = model.Type;
            admin.Username = model.Username;
            _unit.AdminRepository.Add(admin);
            _unit.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordModel model)
        {
            try
            {
                if (Session["Admin"] == null)
                    return RedirectToAction("Index", "Home", "");

                var admin = _unit.AdminRepository.GetAll().ToList().Where(x => x.Username == Session["Admin"].ToString()).FirstOrDefault();
                if (model.NewPassword == model.RePassword && model.OldPassword == admin.Password)
                {
                    admin.Password = model.RePassword;
                    _unit.AdminRepository.Add(admin);
                    _unit.Save();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("ChangePassword");
            }
            catch
            {
                return RedirectToAction("Index", "Home", "");
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}