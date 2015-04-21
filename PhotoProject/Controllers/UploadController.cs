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
            PictureHelper picHelp = new PictureHelper(context);

            var userID = User.Identity.GetUserId();
            UserInfo currentUser = context.UserInfos.Single(emp => emp.UserId == userID);

            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            string fileExtension = picPro.GetFileExtends(file.FileName);
            int size = file.ContentLength;
            string validationStr = picPro.ValidatePicture(fileExtension, size);

            if(validationStr == "Valid")
            {
                //obtain image data
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                //create picture
                Picture pic = picHelp.CreatPicture(userID, formcollection["Title"], Convert.ToDecimal(formcollection["Cost"]), formcollection["Location"], formcollection["Description"], DateTime.Now, fileExtension, data);

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