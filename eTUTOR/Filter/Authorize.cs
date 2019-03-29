using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Controllers;
namespace eTUTOR.Filter
{
    public class Authorize : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","User"},
                    {"Action","Login" }
                });
                
            }
            if (HttpContext.Current.Session["Role"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","User"},
                    {"Action","Login" }
                });

            }
            base.OnActionExecuted(filterContext);
        }
        

    }
}