using Business_Logic;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PhotoProject.Controllers
{
    public class UserHomeController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureProcess picPro = new PictureProcess();
        private static PictureHelper picHelp = new PictureHelper(db);
        private static UserInfoHelper userHelp = new UserInfoHelper();

        // GET: UserHome
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Galary(string ownId)
        {
            var userID = User.Identity.GetUserId();
            //define whether the visiter is the user of this home page
            bool isOwner = userID == ownId ? true : false;

            ViewData["ownedPictures"] = picHelp.GetOwnedPictures(ownId);
            ViewData["likedPictures"] = picHelp.GetLikedPictures(ownId);
            ViewData["Following"] = userHelp.GetFollowing(ownId);
            return View();
        }

    }
}