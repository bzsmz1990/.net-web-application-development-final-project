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
using SendGrid;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Routing;

namespace PhotoProject.Controllers
{
    public class PictureDetailsController : Controller
    {
        private PictureHelper picHelp = new PictureHelper(AlbumDetailsController.db);

        // GET: PictureDetails/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = AlbumDetailsController.db.Pictures.Find(id);
            var userId = User.Identity.GetUserId();
            ViewBag.isOwner = User.Identity.GetUserId() == picture.OwnerId;

            UserInfo userInfo = null;
            if (userId != null)
            {
                userInfo = AlbumDetailsController.db.UserInfos.Single(u => u.UserId == userId);
            }

            ViewBag.LikeAction = userInfo == null ? false : userInfo.LikedPictures.Contains(picture); //This will be true if the user has liked the picture and false if the user has unliked the picture


            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer;
            return View(picture);
        }
                

        [Authorize]
        // GET: PictureDetails/LikePicture
        public ActionResult LikePicture(int id)
        {
            Tuple<Picture, bool> pictureAndLikeAction = picHelp.LikePicture(id, User.Identity.GetUserId());

            if (pictureAndLikeAction == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.LikeAction = pictureAndLikeAction.Item2; //This will be true if the user has liked the picture and false if the user has unliked the picture

            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "PictureDetails", action = "Details", id = pictureAndLikeAction.Item1.Id }));
        }

        [Authorize]
        [HttpPost]
        // POST: PictureDetails/ReportPicture/5
        public async Task<ActionResult> ReportPicture(int id, string reason)
        {
            Picture picture = picHelp.ReportPicture(id);
            if (picture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(picture);
        }


        // GET: PictureDetails/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = AlbumDetailsController.db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }

            return View(picture);
        }

        // POST: PictureDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            Picture dbPicture = AlbumDetailsController.db.Pictures.Find(id);

            if (dbPicture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            bool updateSuccessful = TryUpdateModel(dbPicture, new[] { "Title", "Cost", "Location", "Description" });
            if (updateSuccessful)
            {
                AlbumDetailsController.db.SaveChanges();
          //      return PartialView(dbPicture);
                //return RedirectToAction("Details", "PictureDetails", new { id = id });
              return RedirectToAction("Details", new RouteValueDictionary(new { controller = "PictureDetails", action = "Details", id = dbPicture.Id }));
            }
            else
            {
                return View(dbPicture);
            }


        }



    }
}
