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

namespace PhotoProject.Controllers
{
    public class PictureDetailsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureHelper picHelp = new PictureHelper(db);

        private string PHOTO_PROJECT_EMAIL = "alo270@nyu.edu";

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
        [HttpPost]
        // POST: PictureDetails/LikePicture
        public ActionResult LikePicture(int id)
        {
            Picture picture = picHelp.LikePicture(id, User.Identity.GetUserId());

            if (picture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(picture);
        }

        [Authorize]
        [HttpPost]
        // POST: PictureDetails/ReportPicture/5
        public async Task<ActionResult> ReportPicture(int id, string reason)
        {
            //TODO: ADD USERNAME AND PASSWORD FOR EMAIL
            Picture picture = picHelp.ReportPicture(id);
            if (picture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userID = User.Identity.GetUserId();
            UserInfo userInfo = db.UserInfos.Single(i => i.UserId == userID);

            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(PHOTO_PROJECT_EMAIL);
            myMessage.From = new MailAddress(userInfo.User.Email, userInfo.FullName);
            myMessage.Subject = "Reported Picture: " + picture.Id + " Created By " + picture.Owner.FullName;
            myMessage.Text = reason;

            var credentials = new NetworkCredential("username", "password");

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            await transportWeb.DeliverAsync(myMessage);

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
                return RedirectToAction("Details", picture.Id);
            }

            return View(picture);
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
