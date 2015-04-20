using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Business_Logic;
using PagedList;

namespace PhotoProject.Controllers
{
    public class SearchController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureHelper pictureHelper = new PictureHelper(db);
        private static int LIST_SIZE = 20;

        // GET: Search
        //This method will be used to get all pictures, or get them by a certain
        //order. The allowed orders are: most_recent and most_purchased. This method also allows for 
        //pagination. Please see: http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
        //for instructions of how to do a paged view
        public ActionResult Index(string sortOrder, int? page)
        {
            ICollection<Picture> pictures = null;
            switch (sortOrder)
            {
                case "most_recent":
                    pictures = pictureHelper.GetPicturesOrderedByMostRecent();
                    break;
                case "most_purchased":
                    pictures = pictureHelper.GetPicturesOrderedByMostPurchased();
                    break;
                default: //All Pictures
                    pictures = pictureHelper.GetAllPictures();
                    break;
            }

            int pageSize = LIST_SIZE;
            int pageNumber = (page ?? 1);
            return View(pictures.ToPagedList(pageNumber, pageSize));
        }


        // GET: Search/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Search/Create
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "UserId");
            ViewBag.OwnerId = new SelectList(db.UserInfos, "UserId", "FirstName");
            return View();
        }

        // POST: Search/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OwnerId,Title,Cost,Location,Description,UploadTime,NumberOfLikes,HasBeenReported,OriginalImg,CompressImg,PictureType,Hidden,AlbumId")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "UserId", picture.AlbumId);
            ViewBag.OwnerId = new SelectList(db.UserInfos, "UserId", "FirstName", picture.OwnerId);
            return View(picture);
        }

        // GET: Search/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "UserId", picture.AlbumId);
            ViewBag.OwnerId = new SelectList(db.UserInfos, "UserId", "FirstName", picture.OwnerId);
            return View(picture);
        }

        // POST: Search/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OwnerId,Title,Cost,Location,Description,UploadTime,NumberOfLikes,HasBeenReported,OriginalImg,CompressImg,PictureType,Hidden,AlbumId")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "UserId", picture.AlbumId);
            ViewBag.OwnerId = new SelectList(db.UserInfos, "UserId", "FirstName", picture.OwnerId);
            return View(picture);
        }

        // GET: Search/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Search/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
