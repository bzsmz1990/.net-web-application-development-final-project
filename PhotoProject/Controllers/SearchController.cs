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
using PhotoProject.ViewModels;

namespace PhotoProject.Controllers
{
    public class SearchController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureHelper pictureHelper = new PictureHelper(db);
        private static AlbumHelper albumHelper = new AlbumHelper(db);

        private static int LIST_SIZE = 20;

        // GET: Search
        /* This method will be used to get all pictures/abums, or get them by a certain order.
        /* The allowed orders are: most_recent and most_purchased. This method also allows for 
        /* pagination. Please see: http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
        /* for instructions of how to do a paged view. The results are stored on SearchResultsViewModel
         * 
         * Receives: Word for ordering, current page on pictures result, current page on albums result
         * Returns: SearchResultsViewModel with the pictures and albums that matched the search
         * */
        public ActionResult Index(string sortOrder, int? picturePage, int? albumPage)
        {
            ICollection<Picture> pictures = null;
            ICollection<Album> albums = null;
            switch (sortOrder)
            {
                case "most_recent":
                    pictures = PictureHelper.picHelp.GetPicturesOrderedByMostRecent();
                    albums = AlbumHelper.albumHelp.GetAlbumsOrderedByMostRecent();
                    break;
                case "most_purchased":
                    pictures = pictureHelper.GetPicturesOrderedByMostPurchased();
                    albums = AlbumHelper.albumHelp.GetAlbumsOrderedByMostPurchased();
                    break;
                default: //All Pictures and Albums
                    pictures = pictureHelper.GetAllPictures();
                    albums = AlbumHelper.albumHelp.GetAllAlbums();
                    break;
            }

            int picturePageNumber = (picturePage ?? 1);
            int albumPageNumber = (albumPage ?? 1);

            SearchResultsViewModel viewModel = new SearchResultsViewModel();
            viewModel.Pictures = pictures.ToPagedList(picturePageNumber, LIST_SIZE);
            viewModel.Albums = albums.ToPagedList(albumPageNumber, LIST_SIZE);

            return View(viewModel);
        }

        // GET: Search/SearchResults
        /* This method allows for pictures and albums to be searched by a search term.
         * For the picture, the search term will be matched against Title, Description or Tag.
         * For the album, the search term will be matched against its Name.
         * 
         * Receive: search term, current page on the picture search results, current page in the album search result
         * Returns: SearchResultsViewModel with the pictures and albums that matched the search
         * */
        public ActionResult SearchResults(string searchTerm, int? picturePage, int? albumPage)
        {
            List<Picture> pictures = pictureHelper.GetPicturesWhereTitleHasWord(searchTerm);
            pictures.AddRange(pictureHelper.GetPicturesWhereDescriptionHasWord(searchTerm));
            pictures.AddRange(pictureHelper.GetPicturesWhereTagHasWord(searchTerm));

            ICollection<Album> albums = albumHelper.GetAlbumsWhereTitleHasWord(searchTerm);

            int picturePageNumber = (picturePage ?? 1);
            int albumPageNumber = (albumPage ?? 1);

            SearchResultsViewModel viewModel = new SearchResultsViewModel();
            viewModel.Pictures = pictures.ToPagedList(picturePageNumber, LIST_SIZE);
            viewModel.Albums = albums.ToPagedList(albumPageNumber, LIST_SIZE);

            return View(viewModel);
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
