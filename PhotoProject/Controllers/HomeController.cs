using Business_Logic;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoProject.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureHelper picHelp = new PictureHelper(db);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AllPicture()
        {
            ViewBag.AllPictures = picHelp.GetPicturesOrderedByTitle();
            return View();
        }
    }
}