using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishClass
{
    public class CheckLoginFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string aName = filterContext.ActionDescriptor.ActionName;//获取Action的名称
            string cName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;//获取控件器的名称

            if (cName == "Login" || (cName == "Home" && aName == "Index"))
            {
       
            }
            else
            {
                    if (filterContext.HttpContext.Request.Cookies["UID"] == null)
                    {
                        filterContext.Result = new RedirectResult("/Login/Index2");//页面重定向
                       
                    }
            }
        }
    }
}