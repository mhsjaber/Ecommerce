using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageMemberController : Controller
    {
        private MemberModel memberModel = new MemberModel();
        public ActionResult Index()
        {
            var model = memberModel.Get();
            return View(model);
        }

        public ActionResult Premium()
        {
            var model = memberModel.Get()
                .Where(x => x.Status == Core.Member.CustomerStatus.Premium).ToList();
            return View(model);
        }

        public ActionResult Update(Guid id)
        {
            try
            {
                var model = memberModel.GetDetails(id);
                return View(model);
            }
            catch
            {
                Session["Notify"] = "Failed to load mumber details.";
                Session["Type"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Update(MemberViewModel model)
        {
            try
            {
                memberModel.Update(model);
                Session["Notify"] = "Member updated successfully.";
                Session["Type"] = "success";
            }
            catch
            {
               Session["Notify"] = "Failed to update member.";
               Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                memberModel.Delete(id);
                Session["Notify"] = "Member deleted successfully.";
                Session["Type"] = "success";
            }
            catch
            {
                Session["Notify"] = "Failed to update member.";
                Session["Type"] = "error";
            }
            return RedirectToAction("Index");
        }
    }
}