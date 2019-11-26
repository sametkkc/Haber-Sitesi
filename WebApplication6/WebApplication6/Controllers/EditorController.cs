using DAL;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.MyResult;

namespace WebApplication6.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        EditorRepository _rep;
        // GET: Editor
        public EditorController(EditorRepository rep)
        {
            _rep = rep;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditorList()
        {
            return View(_rep.List());
        }
        public ActionResult Add()
        {
            Editor editor = new Editor();
            return View(editor);
        }
        [HttpPost]
        public ActionResult Add(Editor edtr, HttpPostedFileBase file)
        {
            EditorRepository editorAdd = new EditorRepository();
            edtr.CreateDate = DateTime.Now;
            edtr.ImageId = AddNewsImage(file);
            editorAdd.Add(edtr);

            return RedirectToAction("EditorList");

        }
        public ActionResult Delete(int id)
        {
            EditorRepository edrDel = new EditorRepository();
            edrDel.Remove(id);
            return RedirectToAction("EditorList");
        }
        public ActionResult Update(int id)
        {
            Editor editorUp = _rep.FindById(id);
            return View(editorUp);
        }
        [HttpPost]
        public ActionResult Update(Editor edtUpt)
        {

            edtUpt.CreateDate = DateTime.Now;
            _rep.Update(edtUpt);
            return RedirectToAction("EditorList");
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
            ImageRepository ımageRepository = new ImageRepository();
            Image image = new Image();

            image.FileUrl = path;
            image.Name = file.FileName;
            ımageRepository.UploadImageInDataBase(file, image);

            return ımageRepository.List().FirstOrDefault(c => c.Name == image.Name).Id;
        }

        public MyFileResultEditor ExportToExcel()
        {

            //var news = _rep.List();
            //var grid = new GridView();
            //grid.DataSource = news;
            //grid.DataBind();

            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename-haberler.xls");
            //Response.ContentType = "application/ms-excel";

            //Response.Charset = "";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //grid.RenderControl(htw);

            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
            //return View("MyView");
            var news = _rep.List();
            return new MyFileResultEditor(news);
        }

    }
}