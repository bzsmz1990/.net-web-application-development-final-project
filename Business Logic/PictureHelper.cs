using DataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class PictureHelper
    {
        private ApplicationDbContext db { get; set; }

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
            return db.Pictures.OrderByDescending(p => p.SaleTransactions.Count).ToList();
        }

        public ICollection<Picture> GetPicturesOrderedByMostRecent()
        {
            return db.Pictures.OrderByDescending(p => p.UploadTime).ToList();
        }

        public ICollection<Picture> GetPicturesWhereTitleHasWord(string searchString)
        {
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.ToList();
            }

            return db.Pictures.Where(p => p.Title.Contains(searchString)).ToList();
        }

        public ICollection<Picture> GetPicturesWhereDescriptionHasWord(string searchString)
        {
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.ToList();
            }

            return db.Pictures.Where(p => p.Description.Contains(searchString)).ToList();
        }

        public ICollection<Picture> GetPicturesWhereTagHasWord(string searchString)
        {
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.ToList();
            }


            //TODO: can writ e aquery to reutnr based on relationships?
            return null;
        }

        public void LikePicture (Picture picture, UserInfo user)
        {
            if (!picture.LikedBy.Contains(user))
            {
                picture.NumberOfLikes++;
                picture.LikedBy.Add(user);
                //TODO: ALSO ADD CREDIT TO OWNER
            }
        }

        public void ReportPicture(Picture picture, UserInfo user)
        {
            picture.HasBeenReported = true;
            //TODO: SEND NOTIFICATION
        }
        
    }
}
