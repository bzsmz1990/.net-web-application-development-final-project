using Business_Logic;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoProject.ViewModels;

namespace PhotoProject.Controllers
{
    public class UserHomeController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureProcess picPro = new PictureProcess();
        private static PictureHelper picHelp = new PictureHelper(db);
        private static UserInfoHelper userHelp = new UserInfoHelper(db);

        // GET: UserHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Gallery(string id)
        {
            var userID = User.Identity.GetUserId();
            //define whether the visiter is the user of this home page
            bool isOwner = userID == id ? true : false;

            if(id==null)    //if the parameter id is null
            {
                return RedirectToAction("Error", "Upload", new { errorMessage = "Didn't give user id parameter!" });
            }
            else
            {
                ViewBag.OwnedPictures = picHelp.GetOwnedPictures(id);
                ViewBag.LikedPictures = picHelp.GetLikedPictures(id);
                ViewBag.Following = userHelp.GetFollowing(id);
                //UserHomeViewModel userhome = new UserHomeViewModel();
                //userhome.userId = id;
                //userhome.OwnedPictures = picHelp.GetOwnedPictures(id);
                //userhome.LikedPictures = picHelp.GetLikedPictures(id);
                //userhome.Following = userHelp.GetFollowing(id);
                return View();
            }
            
        }

    }
}