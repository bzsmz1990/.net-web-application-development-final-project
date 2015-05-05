using Business_Logic;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoProject.ViewModels;
using System.Net;

namespace PhotoProject.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private static PictureProcess picPro = new PictureProcess();
        private static PictureHelper picHelp = new PictureHelper(AlbumDetailsController.db);
        private static UserInfoHelper userHelp = new UserInfoHelper(AlbumDetailsController.db);
        private static AlbumHelper albumHelp = new AlbumHelper(AlbumDetailsController.db);

        // GET: UserHome
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult Error(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Gallery(string id)
        {
            var userID = User.Identity.GetUserId();
            //define whether the visiter is the user of this home page
            bool isOwner = userID == id ? true : false;
            ViewBag.isOwner = isOwner;
            if(id==null)    //if the parameter id is null
            {
                return RedirectToAction("Error", "UserHome", new { errorMessage = "Didn't give user id parameter!" });
            }
            else
            {
                GalleryViewModel userhome = new GalleryViewModel();
                userhome.isOwner = isOwner;
                userhome.Owner = userHelp.GetUser(id);
                userhome.OwnedPictures = picHelp.GetOwnedPictures(id);
                userhome.LikedPictures = picHelp.GetLikedPictures(id);
                userhome.Following = userHelp.GetFollowing(id);
                if (userID != null)
                {
                    UserInfo userInfo = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
                    //THis will be true if the current user has followed *this* user, false otherwise
                    ViewBag.FollowAction = userInfo == null ? false : userInfo.Following.Contains(AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == id));
                }
                else
                {
                    ViewBag.FollowAction = false;
                }
                return View(userhome);
            }
            
        }

        public ActionResult FollowUser(string id)
        {
            if (id == null)   
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuple<UserInfo, bool> userAndFollowAction = userHelp.FollowUser(User.Identity.GetUserId(), id);

            ViewBag.FollowAction = userAndFollowAction.Item2; //This will be true if the user has followed another and false if he has unfollowed.

            //Return to whichever page that called it
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult CreateNewAlbum()
        {
            CreateAlbumViewModel createalbum = new CreateAlbumViewModel();
            var userID = User.Identity.GetUserId();
            UserInfo currentUser = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
            createalbum.owner = currentUser;
            return View(createalbum);
        }

        [HttpPost]
        public ActionResult CreateNewAlbum(FormCollection formcollection)
        {
            CreateAlbumViewModel createalbum = new CreateAlbumViewModel();
            createalbum.album = new Album();
            createalbum.album.Pictures = new List<Picture>();
            createalbum.owner = new UserInfo();
            var userID = User.Identity.GetUserId();
            UserInfo currentUser = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
            createalbum.owner = currentUser;

            List<Picture> picInAlbum = new List<Picture>();
            foreach (var key in formcollection.Keys)
            {
                if (key.ToString().StartsWith("Picture"))
                {
                    int picId = int.Parse(key.ToString().Replace("Picture", ""));
                    Picture pic = currentUser.OwnedPictures.Single(p=>p.Id==picId);
                    if (formcollection[key.ToString()].Contains("true"))
                    {
                        if(pic.AlbumId == null)
                            picInAlbum.Add(pic);
                    }        
                }
            }
            createalbum.album = albumHelp.CreateAlbum(formcollection["album.Name"], userID, currentUser, picInAlbum, Convert.ToDecimal(formcollection["album.Cost"]));
            currentUser.Albums.Add(createalbum.album);
            AlbumDetailsController.db.SaveChanges();
            return RedirectToAction("Gallery", "UserHome", new { id = userID });
        }
    
        [Authorize]
        public ActionResult OwnedPictures()
        {
            var userID = User.Identity.GetUserId();
            UserInfo user = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);

            List<Transaction> trans = AlbumDetailsController.db.Transactions.Where(tr => tr.BuyerId == userID).ToList();
            List<Picture> pics = new List<Picture>();
            List<Album> albums = new List<Album>();
            if (trans != null)
            {
                
                foreach (Transaction tr in trans)
                {
                    foreach (Picture pic in tr.PicturesBeingSold)
                    {
                        if (!pics.Contains(pic))
                        {
                            pics.Add(pic);
                        }
                    }
                }
                
                foreach (Transaction tr in trans)
                {
                    foreach (Album al in tr.AlbumsBeingSold)
                    {
                        if (!albums.Contains(al))
                        {
                            albums.Add(al);
                        }
                    }
                }
            }
            return View(Tuple.Create(pics, albums));
        }
    }
}