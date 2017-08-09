using ECommerce.Core;
using ECommerce.Core.Member;
using ECommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProfileController : Controller
    {
        private ECommerceUnitOfWork _unit = new ECommerceUnitOfWork(new ECommerceContext());
        public ActionResult Index()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login");

            var user = _unit.CustomerRepository.GetAll()
                    .Where(x => x.Username == Session["Username"].ToString())
                    .FirstOrDefault();
            var model = new ProfileModel();
            model.Address = user.Address;
            model.Email = user.Email;
            model.FullName = user.FullName;
            model.Mobile = user.Mobile;
            model.Username = user.Username;
            return View(model);
        }

        public ActionResult Login()
        {
            if (Session["Username"] != null)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var user = _unit.CustomerRepository.GetAll()
                   .Where(x => x.Username == model.Username && x.Password == model.Password)
                   .FirstOrDefault();

            if (user != null)
            {
                Session["Username"] = model.Username;
                if (Session["InvoiceID"] != null)
                {
                    var invoice = _unit.InvoiceRepository.GetById(Guid.Parse(Session["InvoiceID"].ToString()));
                    invoice.CustomerID = user.ID;
                    _unit.InvoiceRepository.Update(invoice);
                    _unit.Save();
                }
                return RedirectToAction("Index");
            }
            else
            {
                Session["Notify"] = "Username and password not matching.";
                Session["Type"] = "error";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (model.NewPassword != model.RePassword)
            {
                Session["Notify"] = "Password and Confirm Password not matching.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }

            var user = _unit.CustomerRepository.GetAll()
                   .Where(x => x.Username == Session["Username"].ToString() && x.Password == model.OldPassword)
                   .FirstOrDefault();

            if (user == null)
            {
                Session["Notify"] = "Old Password not matching.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
            else
            {
                user.Password = model.NewPassword;
                _unit.CustomerRepository.Update(user);
                _unit.Save();
                Session["Notify"] = "Password changed successfully.";
                Session["Type"] = "success";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (model.Password != model.RePassword)
            {
                Session["Notify"] = "Password and Confirm Password not matching.";
                Session["Type"] = "error";
                return RedirectToAction("Login");
            }

            var user = _unit.CustomerRepository.GetAll()
                   .Where(x => x.Username.ToLower() == model.Username.ToLower())
                   .FirstOrDefault();

            if (user == null)
            {
                var customer = new Customer();
                customer.Address = model.Address;
                customer.Email = model.Email;
                customer.FullName = model.FullName;
                customer.Mobile = model.Mobile;
                customer.Password = model.Password;
                customer.Status = CustomerStatus.Temporary;
                customer.Username = model.Username;
                customer.CreatedOn = DateTime.Now;
                _unit.CustomerRepository.Add(customer);
                _unit.Save();
                Session["Username"] = model.Username;
                if (Session["InvoiceID"] != null)
                {
                    var invoice = _unit.InvoiceRepository.GetById(Guid.Parse(Session["InvoiceID"].ToString()));
                    invoice.CustomerID = customer.ID;
                    _unit.InvoiceRepository.Update(invoice);
                    _unit.Save();
                }
                Session["Notify"] = "User registered successfully.";
                Session["Type"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                Session["Notify"] = "Username already taken.";
                Session["Type"] = "error";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["Username"] = null;
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult UpdateProfile(ProfileModel model)
        {
            try
            {
                var user = _unit.CustomerRepository.GetAll()
                        .Where(x => x.Username == Session["Username"].ToString())
                        .FirstOrDefault();

                user.Address = model.Address;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.Mobile = model.Mobile;
                user.Username = model.Username;

                _unit.CustomerRepository.Update(user);
                _unit.Save();
                Session["Username"] = model.Username;

                Session["Notify"] = "Profile updated successfully.";
                Session["Type"] = "success";
            }
            catch(Exception ex)
            {
                Session["Notify"] = "Error in profile update.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }
    }
}