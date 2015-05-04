using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace PhotoProject.Controllers
{
    public class AlbumDetailsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        // GET: AlbumDetail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlbumDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return View(album);
        }
    }
}