using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class CustomAuthorize: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["Admin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { area = "admin", controller = "AdminLogin", action = "index" }));
            }
        }
    }
}