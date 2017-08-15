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
            try
            {
                var admin = _unit.AdminRepository.GetById(model.ID);
                if (Session["AdminType"].ToString() != "SuperAdmin" && Session["Admin"].ToString() != admin.Username)
                {
                    Session["Notify"] = "You don't have permission to edit profile.";
                    Session["Type"] = "error";
                    return RedirectToAction("Index");
                }
                admin.Name = model.Name;
                if (admin.Type == Core.Member.AdminType.SuperAdmin)
                    admin.Type = model.Type;
                admin.Username = model.Username;
                Session["Admin"] = model.Username;
                _unit.AdminRepository.Update(admin);
                _unit.Save();
                Session["Notify"] = "Profile update successfully.";
                Session["Type"] = "success";
            }
            catch (Exception ex)
            {
                Session["Notify"] = "Error in profile update.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            if (Session["AdminType"].ToString() != "SuperAdmin")
            {
                Session["Notify"] = "You cannot create an admin..";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Core.Member.Admin model)
        {
            try
            {
                var admin = new Core.Member.Admin();
                admin.Name = model.Name;
                admin.Password = model.Password;
                admin.Type = model.Type;
                admin.Username = model.Username;
                admin.CreatedOn = DateTime.Now;
                _unit.AdminRepository.Add(admin);
                _unit.Save();
                Session["Notify"] = "Admin created successfully.";
                Session["Type"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["Notify"] = "Error in admin create.";
                Session["Type"] = "error";
                return RedirectToAction("Create");
            }
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
                    Session["Notify"] = "Password changed successfully";
                    Session["Type"] = "success";
                    return RedirectToAction("Index");
                }
                else if (model.NewPassword != model.RePassword)
                {
                    Session["Notify"] = "New password & confirm password not matching.";
                    Session["Type"] = "error";
                }
                else if (model.OldPassword != admin.Password)
                {
                    Session["Notify"] = "Old password not matching.";
                    Session["Type"] = "error";
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
            Session["AdminType"] = null;
            return RedirectToAction("Index", "AdminLogin");
        }
    }
}