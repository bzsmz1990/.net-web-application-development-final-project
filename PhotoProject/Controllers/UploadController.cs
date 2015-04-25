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
    [Authorize]
    public class UploadController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureProcess picPro = new PictureProcess();
        private static PictureHelper picHelp = new PictureHelper(db);

        // GET: Upload
        public ActionResult Index()
        {
            //should direct to RedirectToAction("Galary", "UserHome");
            return View();
        }

        public ActionResult Error(string errorMessage)
        {
            ViewBag.Message = errorMessage;
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
            UserInfo currentUser = db.UserInfos.Single(emp => emp.UserId == userID);

            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            string fileExtension = picPro.GetFileExtends(file.FileName).ToLower(); ;
            int size = file.ContentLength;
            string validationStr = picPro.ValidatePicture(fileExtension, size);

            if(validationStr == "Valid")
            {
                //obtain image data
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);

                Picture.ValidFileType type = (Picture.ValidFileType)Enum.Parse(typeof(Picture.ValidFileType), fileExtension);

                //create picture
                Picture pic = picHelp.CreatPicture(userID, formcollection["Title"], Convert.ToDecimal(formcollection["Cost"]), formcollection["Location"], formcollection["Description"], DateTime.Now, type, data);

                db.Pictures.Add(pic);
                currentUser.OwnedPictures.Add(pic);
                db.SaveChanges();

                return RedirectToAction("Galary", "UserHome", new { id = userID });
            }
            else
            {
                ModelState.AddModelError("CustomError", validationStr);
                return RedirectToAction("Error", "Upload", new { errorMessage = validationStr });
            }   
        }
    }
}