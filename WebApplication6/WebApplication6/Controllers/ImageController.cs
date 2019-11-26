using DAL.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace WebApplication6.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ImageUpload()
        {

            return View();
        }

        [HttpPost]

        public ActionResult ImageUpload(HttpPostedFileBase file)
        {
            var path = "";

            if (file != null)
            {
                if (file.ContentLength > 0)
                {


                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(file.FileName).ToLower() == ".png"
                        || Path.GetExtension(file.FileName).ToLower() == ".gif"
                        || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                    {
                        path = Path.Combine(Server.MapPath("~/img"), file.FileName);
                        file.SaveAs(path);
                        ViewBag.UploadSuccess = true;
                    }

                }
            }
            ImageRepository ımageRepository = new ImageRepository();
            Image image = new Image();

            image.FileUrl = path;
            image.Name = file.FileName;
            ımageRepository.UploadImageInDataBase(file, image);

            return View();
        }
    }
}