using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationOutage.Filters
{
    public class AdminAuthorize: AuthorizeAttribute
    {
        public AdminAuthorize()
        {
            this.Users = System.Configuration.ConfigurationManager.AppSettings["AdminUser"].ToString();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "Unauthorized" };
        }
    }
}