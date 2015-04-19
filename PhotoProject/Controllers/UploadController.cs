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
            PictureProcess picPro = new PictureProcess();

            var userID = User.Identity.GetUserId();
            UserInfo currentUser = context.UserInfos.Single(emp => emp.UserId == userID);

            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            string fileExtension = picPro.GetFileExtends(file.FileName);
            int size = file.ContentLength;

            Picture pic = new Picture();
            pic.OwnerId = userID;
            pic.Title = formcollection["Title"];
            pic.Cost = Convert.ToDecimal(formcollection["Cost"]);
            pic.Location = formcollection["Location"];
            pic.Description = formcollection["Description"];
            pic.UploadTime = DateTime.Now;
            pic.PictureType = fileExtension;
            

            string validationStr = picPro.ValidatePicture(pic.PictureType, size);
            if (validationStr == "Valid")
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                pic.OriginalImg = data;
                pic.CompressImg = data;

                context.Pictures.Add(pic);
                currentUser.OwnedPictures.Add(pic);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("CustomError", validationStr);
                return View();
            }   
        }
    }
}