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
            var model = memberModel.GetDetails(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(MemberViewModel model)
        {
            memberModel.Update(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            memberModel.Delete(id);
            return RedirectToAction("Index");
        }
    }
}