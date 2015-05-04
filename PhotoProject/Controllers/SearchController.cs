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

        private static int LIST_SIZE = 10;

        // GET: Search
        /* This method will be used to get all pictures/abums, or get them by a certain order.
        /* The allowed orders are: most_recent (default behavior if no order is provided and most_purchased. This method also allows for 
        /* pagination. Please see: http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
        /* for instructions of how to do a paged view. The results are stored on SearchResultsViewModel
         * 
         * Receives: Word for ordering, current page on pictures result, current page on albums result
         * Returns: SearchResultsViewModel with the pictures and albums that matched the search
         * */
        public ActionResult Index(string sortOrder, string currentFilter, int? picturePage, int? albumPage)
        {
            if (sortOrder != null)
            {
                picturePage = 1;
            }
            else
            {
                sortOrder = currentFilter;
            }
            ViewBag.CurrentFilter = sortOrder;

            ICollection<Picture> pictures = null;
            ICollection<Album> albums = null;

            switch (sortOrder)
            {
                case "most_purchased":
                    pictures = pictureHelper.GetPicturesOrderedByMostPurchased();
                    albums = albumHelper.GetAlbumsOrderedByMostPurchased();
                    break;
                case "most_recent":
                case null:
                default:
                    pictures = pictureHelper.GetPicturesOrderedByMostRecent();
                    albums = albumHelper.GetAlbumsOrderedByMostRecent();
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
        public ActionResult SearchResults2(string searchTerm, string currentFilter, int? picturePage, int? albumPage)
        {
            if (searchTerm != null)
            {
                picturePage = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }
            ViewBag.CurrentFilter = searchTerm;
            
            HashSet<Picture> pictures = new HashSet<Picture>();
            pictures.UnionWith(pictureHelper.GetPicturesWhereTitleHasWord(searchTerm));
            pictures.UnionWith(pictureHelper.GetPicturesWhereDescriptionHasWord(searchTerm));
            pictures.UnionWith(pictureHelper.GetPicturesWhereTagHasWord(searchTerm));

            ICollection<Album> albums = albumHelper.GetAlbumsWhereTitleHasWord(searchTerm);

            int picturePageNumber = (picturePage ?? 1);
            int albumPageNumber = (albumPage ?? 1);

            SearchResultsViewModel viewModel = new SearchResultsViewModel();
            viewModel.Pictures = pictures.ToPagedList(picturePageNumber, LIST_SIZE);
            viewModel.Albums = albums.ToPagedList(albumPageNumber, LIST_SIZE);

            return View(viewModel);
        }
        public ActionResult FilterSearchResults(string searchTerm, DataLayer.Picture.ValidFileType pictureType, int? picturePage, int? albumPage)
        {
            if (searchTerm == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int picturePageNumber = (picturePage ?? 1);
            int albumPageNumber = (albumPage ?? 1);

            //TODO: Remove need to re-do search
            SearchResultsViewModel viewModel = new SearchResultsViewModel();
            viewModel.Pictures = pictureHelper.FilterListByPictureType(searchTerm, pictureType).ToPagedList(picturePageNumber, LIST_SIZE);
            viewModel.Albums = albumHelper.FilterListByPictureType(searchTerm, pictureType).ToPagedList(albumPageNumber, LIST_SIZE);


            return View(viewModel);
        }



        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
