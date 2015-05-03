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
            Picture picture = picHelp.ReportPicture(id);
            if (picture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

            return PartialView(picture);
        }

        // POST: PictureDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            Picture dbPicture = db.Pictures.Find(id);

            if (dbPicture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            bool updateSuccessful = TryUpdateModel(dbPicture, new[] { "Title", "Cost", "Location", "Description" });
            if (updateSuccessful)
            {
                db.SaveChanges();
                return PartialView(dbPicture);
              /*  return RedirectToAction("Details", new RouteValueDictionary(new { controller = "PictureDetails", action = "Details", id = dbPicture.Id }));*/
            }
            else
            {
                return PartialView(dbPicture);

            }


        }



    }
}
