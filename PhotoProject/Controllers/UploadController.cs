using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Business_Logic;

namespace PhotoProject.Controllers
{
    public class UploadController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(FormCollection formcollection)
        {
            var userID = User.Identity.GetUserId();

            Picture pic = new Picture();
            pic.OwnerId = userID;
            pic.Title = formcollection["Title"];
            pic.Cost = Convert.ToDecimal(formcollection["Cost"]);
            pic.Location = formcollection["Location"];
            pic.Description = formcollection["Description"];
            pic.UploadTime = DateTime.Now;
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            string fileExtension = PictureProcess.GetFileExtends(file.FileName);
            pic.PictureType = fileExtension;

            //Apply validation here
            if (file.ContentType != "image/jpeg")
            {
                if (!PictureProcess.CheckFileExtends(fileExtension))
                {
                    ModelState.AddModelError("CustomError", "File type is invalid! We accept jpg, bmp, cr2, nef, arw, pef, dng");
                    return View();
                }
            }

            if (file.ContentLength < (2 * 1024 * 1024))
            {
                ModelState.AddModelError("CustomError", "File size shoud be bigger than 2MB");
                return View();
            }

            byte[] data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);
            pic.OriginalImg = data;
            pic.CompressImg = data;

            context.Pictures.Add(pic);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}