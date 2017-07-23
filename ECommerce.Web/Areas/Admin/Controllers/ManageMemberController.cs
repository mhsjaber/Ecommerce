using ECommerce.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Web.Areas.Admin.Controllers
{
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
    }
}