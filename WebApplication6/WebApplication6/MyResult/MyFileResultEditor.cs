using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace WebApplication6.MyResult
{
    public class MyFileResultEditor :ActionResult
    {
        List<Editor> _liste;
        public MyFileResultEditor(List<Editor> Liste)
        {
            _liste = Liste;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.ClearContent();
            context.RequestContext.HttpContext.Response.Buffer = true;
            context.RequestContext.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=Editors.xls");
            context.RequestContext.HttpContext.Response.ContentType = "application/ms-excel";

            context.RequestContext.HttpContext.Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            string rows = "";
            foreach (var item in _liste)
            {
                rows += string.Format(@"<tr>
                <td>{0} </td><td>{1}</td><td>{2}</td><td>{3}</td>
                </tr>", item.Id, item.Name, item.Password, item.CreateDate);
            }
            string result = @"<div>
<table cellspacing='0' rules='all' border='1' style='border - collapse:collapse;'>
<tr>
            <th scope='col'>Id</th>
            <th scope='col'>Name</th>
            <th scope='col'>Password</th>
            <th scope='col'>CreateDate</th>
                </tr>"
+ rows + @"</table>
</div>";

            context.RequestContext.HttpContext.Response.Output.Write(result);
            context.RequestContext.HttpContext.Response.Flush();
            context.RequestContext.HttpContext.Response.End();

        }
    }
}