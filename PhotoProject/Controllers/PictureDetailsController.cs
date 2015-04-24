using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Microsoft.AspNet.Identity;
using Business_Logic;

namespace PhotoProject.Controllers
{
    public class PictureDetailsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureHelper picHelp = new PictureHelper(db);

        // GET: PictureDetails/Details/5
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

        [Authorize]
        // GET: PictureDetails/Details/5
        public ActionResult LikePicture(int? id)
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

            picHelp.LikePicture(picture, db.UserInfos.Find(User.Identity.GetUserId()));

            return View(picture);
        }

        [Authorize]
        // GET: PictureDetails/ReportPicture/5
        public ActionResult ReportPicture(int? id)
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

            //TODO: DO I NEED TO DEFINE THAT VIEW NEEDS TO PROVIDE BOX FOR REASON?
            return View(picture);
        }

        [Authorize]
        [HttpPost]
        // POST: PictureDetails/ReportPicture/5
        public ActionResult ReportPicture(int id, string reason)
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

            //TODO: DO WE WANT TO SEND ANY EMAIL WITH THE REASON?
            picHelp.ReportPicture(picture, db.UserInfos.Find(User.Identity.GetUserId()));

            return View(picture);
        }
        

        // GET: PictureDetails/Edit/5
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

        // POST: PictureDetails/Edit/5
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

        // GET: PictureDetails/Delete/5
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

        // POST: PictureDetails/Delete/5
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
