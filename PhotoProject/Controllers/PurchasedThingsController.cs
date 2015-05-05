using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Business_Logic;

namespace PhotoProject.Controllers
{
    public class PurchasedThingsController : Controller
    {
        private static PictureHelper picHelp = new PictureHelper(AlbumDetailsController.db);
        // GET: PurchasedThings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OriginalImg(int id)
        {
            Picture pic = new Picture();
            pic = picHelp.GetSpecificPicture(id);
            return View(pic);
        }
    }
}