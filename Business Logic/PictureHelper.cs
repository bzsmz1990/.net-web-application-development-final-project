using DataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class PictureHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static PictureProcess picPro = new PictureProcess();
        public static PictureHelper picHelp = new PictureHelper(db);

        public PictureHelper(ApplicationDbContext context)
        {
            db = context;
        }

        public ICollection<Picture> GetAllPictures()
        {
            return db.Pictures.ToList();
        }

        public ICollection<Picture> GetPicturesOrderedByMostPurchased()
        {
             return (db.Pictures.Where(p => p.SaleTransactions != null).OrderByDescending(p => p.SaleTransactions.Count)
                .Union(db.Pictures.Where(p => p.SaleTransactions == null).OrderByDescending(p => p.UploadTime))).ToList();
        }

        public ICollection<Picture> GetPicturesOrderedByMostRecent()
        {
            return db.Pictures.OrderByDescending(p => p.UploadTime).ToList();
        }

        public List<Picture> GetPicturesWhereTitleHasWord(string searchString)
        {
            
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.OrderBy(p => p.UploadTime).ToList();
            }

            searchString = searchString.ToLower();

            return db.Pictures.Where(p => p.Title.ToLower().Contains(searchString)).OrderBy(p => p.UploadTime).ToList();
        }

        public List<Picture> GetPicturesWhereDescriptionHasWord(string searchString)
        {
            
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.OrderBy(p => p.UploadTime).ToList();
            }

            searchString = searchString.ToLower();

            return db.Pictures.Where(p => p.Description.ToLower().Contains(searchString)).OrderBy(p => p.UploadTime).ToList();
        }

        public List<Picture> GetPicturesWhereTagHasWord(string searchString)
        {
            
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.OrderBy(p => p.UploadTime).ToList();
            }

            searchString = searchString.ToLower();

            return db.Pictures
                .Where(p => p.Tags != null && p.Tags.Any(t => t.Description.ToLower().Contains(searchString)))
                .ToList();
        }

        public Picture LikePicture(int? pictureId, string userId)
        {
            Picture picture = db.Pictures.Find(pictureId);
            UserInfo userInfo = db.UserInfos.Find(userId);

            if (picture == null || userInfo == null)
            {
                return null;
            }

            if (!picture.LikedBy.Contains(userInfo))
            {
                picture.NumberOfLikes++;
                picture.LikedBy.Add(userInfo);
                db.SaveChanges();
                //TODO: ALSO ADD CREDIT TO OWNER
            }

            return picture;
        }

        public Picture ReportPicture(int pictureId)
        {
            Picture picture = db.Pictures.Single(p => p.Id == pictureId);

            if (picture == null)
            {
                return null;
            }

            picture.HasBeenReported = true;
            db.SaveChanges();

            return picture;
        }

        public Picture CreatPicture(string userID, string title, decimal cost,
            string location, string description, DateTime time, string type,
            byte[] data)
        {
            Picture pic = new Picture();
            pic.OwnerId = userID;
            pic.Title = title;
            pic.Cost = cost;
            pic.Location = location;
            pic.Description = description;
            pic.UploadTime = time;
            pic.PictureType = type;
            pic.OriginalImg = data;
            pic.CompressImg = picPro.ZoomAuto(data);
            return pic;
        }

        public ICollection<Picture> GetOwnedPictures(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            return user.OwnedPictures.ToList();
        }

        public ICollection<Picture> GetLikedPictures(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            return user.LikedPictures.ToList();
        }

    }
}
