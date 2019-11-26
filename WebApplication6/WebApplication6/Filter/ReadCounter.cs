using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication6.Filter
{
    public class ReadCounter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int id = Convert.ToInt32(filterContext.ActionParameters["id"]);
            var deger = new NewsRepository();
            deger.ReadCount(id);
        }
    }
}