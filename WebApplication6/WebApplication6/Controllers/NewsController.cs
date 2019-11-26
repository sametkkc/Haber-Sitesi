using DAL;
using DAL.Repository;
using WebApplication6.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Filter;

using WebApplication6.MyResult;

namespace WebApplication6.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        NewsRepository _rep;
        public NewsController(NewsRepository rep)
        {
            _rep = rep;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News(string SearchString)
        {
            var db = new ProjeHaberDbEntities();
            var kayitlar = from a in db.HaberTbl select a;
            if(!String.IsNullOrEmpty(SearchString))
            {
                kayitlar = kayitlar.Where(a => a.title.Contains(SearchString) || a.content.Contains(SearchString));

            }
            return View(kayitlar.ToList());
        }
        [ReadCounter]
        public PartialViewResult NewsDevam(int id)
        {
           return PartialView(_rep.FindById(id));

        }

        public ActionResult NewsList()
        {
            if (Session["user"] == null)
            {

                return RedirectToAction("Login", "Login");
            }
            return View(_rep.List());
        }

        public ActionResult AddNews()
        {
            if (Session["user"] == null)
            {

                return RedirectToAction("News", "News");
            }
            ProjeHaberDbEntities db = new ProjeHaberDbEntities();

            List<SelectListItem> degerler = (from i in db.Editor.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Name,
                                                 Value = i.Id.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            NewsViewModel newsViewModel = new NewsViewModel();
            return View(newsViewModel);
        }
        [HttpPost]
        public ActionResult AddNews(NewsViewModel hbr, HttpPostedFileBase file)
        {
            if (Session["user"] == null)
            {

                return RedirectToAction("News", "News");
            }
            var newsEntity = new HaberTbl();

            
            newsEntity.Id = hbr.Id;
            newsEntity.spot = hbr.spot;
            newsEntity.title = hbr.title;
            newsEntity.content = hbr.content;
            newsEntity.ImageId = AddNewsImage(file);

            ProjeHaberDbEntities db = new ProjeHaberDbEntities();
            var ktg = db.Editor.Where(m => m.Id == hbr.EditorId).FirstOrDefault();
            newsEntity.EditorId = ktg.Id;

            _rep.Add(newsEntity);

            return RedirectToAction("News");

        }


        public ActionResult Delete(int id)
        {
            if (Session["user"] == null)
            {

                return RedirectToAction("News", "News");
            }
            HaberTbl haberTblDelete = _rep.FindById(id);
            _rep.Remove(haberTblDelete.Id);
            return RedirectToAction("NewsList");
        }


        public ActionResult Update(int id)
        {
            ProjeHaberDbEntities db = new ProjeHaberDbEntities();

            List<SelectListItem> degerler = (from i in db.Editor.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Name,
                                                 Value = i.Id.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            return View(_rep.FindById(id));
        }
        [HttpPost]
        public ActionResult Update(HaberTbl hbr,HttpPostedFileBase file)
        {
            if (Session["user"] == null)
            {
                 
                return RedirectToAction("News", "News");
            }
            ProjeHaberDbEntities db = new ProjeHaberDbEntities();
            var ktg = db.Editor.Where(m => m.Id == hbr.EditorId).FirstOrDefault();
            hbr.EditorId = ktg.Id;
           
            hbr.CreateDate = ktg.CreateDate;
            int result = AddNewsImage(file);

            hbr.ImageId = result == 0 ? ktg.ImageId : result;
            
            _rep.Update(hbr);
             return RedirectToAction("NewsList");
        }

        public int AddNewsImage(HttpPostedFileBase file)
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
                        path = Path.Combine(Server.MapPath("~/Content"), file.FileName);
                        file.SaveAs(path);
                        ViewBag.UploadSuccess = true;
                    }

                }
            }
            else
            {
                return 0;
            }
            ImageRepository ımageRepository = new ImageRepository();
            Image image = new Image();

            image.FileUrl = path;
            image.Name = file.FileName;
            ımageRepository.UploadImageInDataBase(file, image);

            return ımageRepository.List().FirstOrDefault(c => c.Name == image.Name).Id;
        }

        public MyFileResult ExportToExcel()
        {
            var news = _rep.List();
            return new MyFileResult(news);
        }

        

    }
}