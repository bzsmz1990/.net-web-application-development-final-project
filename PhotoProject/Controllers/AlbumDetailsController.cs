using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using PhotoProject.ViewModels;
using Microsoft.AspNet.Identity;

namespace PhotoProject.Controllers
{
    [Authorize]
    public class AlbumDetailsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        // GET: AlbumDetail
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
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

        [HttpGet]
        public ActionResult AddPicInAlbum(int? id)
        {
            Album currentalbum = db.Albums.Find(id);

            return View(currentalbum);
        }

        [HttpPost]
        public ActionResult AddPicInAlbum(FormCollection formcollection)
        {
            Album currentalbum = db.Albums.Find(formcollection["Id"]);
            var userID = User.Identity.GetUserId();
            UserInfo currentUser = db.UserInfos.Single(emp => emp.UserId == userID);
            foreach (var key in formcollection.Keys)
            {
                if (key.ToString().StartsWith("Picture"))
                {
                    int picId = int.Parse(key.ToString().Replace("Picture", ""));
                    Picture pic = currentUser.OwnedPictures.Single(p => p.Id == picId);
                    if (formcollection[key.ToString()].Contains("true"))
                    {
                        if (!currentalbum.Pictures.Any(p => p.Id == picId))
                            currentalbum.Pictures.Add(pic);
                    }
                }
            }
            currentalbum.Cost = Convert.ToDecimal(formcollection["Cost"]);
            db.SaveChanges();

            return RedirectToAction("AlbumDetails", "AlbumDetails", new { id = currentalbum.Id });
        }
    }
}