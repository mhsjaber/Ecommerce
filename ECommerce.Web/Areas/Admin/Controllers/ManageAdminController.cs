using ECommerce.Core;
using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
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


        public ActionResult EditProfile()
        {
            var model = _unit.AdminRepository.GetAll().ToList().Where(x=> x.Username == Session["Admin"].ToString()).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Core.Member.Admin model)
        {
            var admin = _unit.AdminRepository.GetById(model.ID);
            if (Session["AdminType"].ToString() != "SuperAdmin" && Session["Admin"].ToString() != admin.Username)
                return RedirectToAction("Index");

            admin.Name = model.Name;
            if(admin.Type == Core.Member.AdminType.SuperAdmin)
                admin.Type = model.Type;
            admin.Username = model.Username;
            Session["Admin"] = model.Username;
            _unit.AdminRepository.Update(admin);
            _unit.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
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
            admin.CreatedOn = DateTime.Now;
            _unit.AdminRepository.Add(admin);
            _unit.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordModel model)
        {
            try
            {
                var admin = _unit.AdminRepository.GetAll().ToList().Where(x => x.Username == Session["Admin"].ToString()).FirstOrDefault();
                if (model.NewPassword == model.RePassword && model.OldPassword == admin.Password)
                {
                    admin.Password = model.RePassword;
                    _unit.AdminRepository.Update(admin);
                    _unit.Save();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("ChangePassword");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Index", "AdminLogin");
        }
    }
}