﻿using DataLayer;
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
            return db.Pictures.Where(p => p.Hidden == false).ToList();
        }

        public ICollection<Picture> GetPicturesOrderedByTitle()
        {
            return db.Pictures.Where(p => p.Hidden == false).OrderBy(p=>p.Title).ToList();
        }

        public ICollection<Picture> GetPicturesOrderedByMostPurchased()
        {
            return (db.Pictures.Where(p => p.SaleTransactions != null && p.Hidden == false).OrderByDescending(p => p.SaleTransactions.Count)
                .Union(db.Pictures.Where(p => p.SaleTransactions == null && p.Hidden == false).OrderByDescending(p => p.UploadTime))).ToList();
        }

        public ICollection<Picture> GetPicturesOrderedByMostRecent()
        {
            return db.Pictures.Where(p => p.Hidden == false).OrderByDescending(p => p.UploadTime).ToList();
        }

        public List<Picture> GetPicturesWhereTitleHasWord(string searchString)
        {
            
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.Where(p => p.Hidden == false).OrderBy(p => p.UploadTime).ToList();
            }

            searchString = searchString.ToLower();

            return db.Pictures.Where(p => p.Title.ToLower().Contains(searchString) && p.Hidden == false).OrderBy(p => p.UploadTime).ToList();
        }

        public List<Picture> GetPicturesWhereDescriptionHasWord(string searchString)
        {
            
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.Where(p => p.Hidden == false).OrderBy(p => p.UploadTime).ToList();
            }

            searchString = searchString.ToLower();

            return db.Pictures.Where(p => p.Description.ToLower().Contains(searchString) && p.Hidden == false).OrderBy(p => p.UploadTime).ToList();
        }

        public List<Picture> GetPicturesWhereTagHasWord(string searchString)
        {
            
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Pictures.Where(p => p.Hidden == false).OrderBy(p => p.UploadTime).ToList();
            }

            searchString = searchString.ToLower();

            return db.Pictures
                .Where(p => p.Tags != null && p.Hidden == false && p.Tags.Any(t => t.Description.ToLower().Contains(searchString)))
                .ToList();
        }

        public Picture LikePicture(int pictureId, string userId)
        {
            Picture picture = db.Pictures.Single(p => p.Id == pictureId);
            UserInfo userInfo = db.UserInfos.Single(u => u.UserId == userId);

            if (picture == null || userInfo == null || picture.Hidden == true)
            {
                return null;
            }

            if (userInfo.LikedPictures != null && userInfo.LikedPictures.Contains(picture))
            {
                userInfo.LikedPictures.Remove(picture);
                picture.NumberOfLikes--;
                db.SaveChanges();
                return picture;
            }

            if (picture.LikedBy == null || !picture.LikedBy.Contains(userInfo))
            {
                picture.NumberOfLikes++;
                picture.LikedBy = (picture.LikedBy ?? new List<UserInfo>());
                picture.LikedBy.Add(userInfo);
                userInfo.LikedPictures = (userInfo.LikedPictures ?? new List<Picture>());
                userInfo.LikedPictures.Add(picture);
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
            string location, string description, DateTime time, Picture.ValidFileType type,
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
            List<Picture> NoHiddenOwnedPictures = user.OwnedPictures.Where(p => p.Hidden == false).ToList();

            return NoHiddenOwnedPictures;
        }

        public ICollection<Picture> GetLikedPictures(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            List<Picture> NoHiddenLikedPictures = user.LikedPictures.Where(p => p.Hidden == false).ToList();
            
            return NoHiddenLikedPictures;
        }

        public int NumPicNotHidden(List<Picture> pictures)
        {
            int num=0;
            foreach(Picture pic in pictures)
            {
                if (!pic.Hidden)
                    num++;
            }
            return num;
        }

        public bool VerifyUserLevel(UserInfo user)
        {
            bool status = true;
            //level 1 <= 10 pictures
            //level 2 <= 20 pictures
            //level 3 <= 30 pictures
            //level 4 unlimited
            if (user.Level == 1 && NumPicNotHidden(user.OwnedPictures.ToList()) >= UserInfoHelper.UserLevelToPictureLimit[1])
                status = false;
            if (user.Level == 2 && NumPicNotHidden(user.OwnedPictures.ToList()) >= UserInfoHelper.UserLevelToPictureLimit[2])
                status = false;
            if (user.Level == 3 && NumPicNotHidden(user.OwnedPictures.ToList()) >= UserInfoHelper.UserLevelToPictureLimit[3])
                status = false;
            return status;
        }

        public ICollection<Picture> FilterListByPictureType(string searchTerm, DataLayer.Picture.ValidFileType PictureType)
        {
            if (searchTerm == null)
            {
                return null;
            }

            List<Picture> pictures = GetPicturesWhereTitleHasWord(searchTerm);
            pictures.AddRange(GetPicturesWhereDescriptionHasWord(searchTerm));
            pictures.AddRange(GetPicturesWhereTagHasWord(searchTerm));

            pictures.RemoveAll(p => p.PictureType == PictureType);

            return pictures;
        }
    }
}
