using DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var ctx = new ProjeHaberDbEntities();
            var usr = ctx.Editor.FirstOrDefault(c => c.Name == model.Name && c.Password == model.Password);

            if(usr!=null)
            {
                Session["user"] = usr;
                return RedirectToAction("NewsList", "News");
            }
            else
            {
                model.Hata = "Name veya Password Hatalı";
                return View("Login", model);
            }
        }

        public ActionResult Logout()
            {
                Session["user"] = null;
                return RedirectToAction("Login");
            }

        }
    }
