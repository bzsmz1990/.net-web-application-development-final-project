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
using System.Data.Entity.Validation;

namespace Business_Logic
{
    public class PictureHelper
    {
        private ApplicationDbContext db;
        private static PictureProcess picPro = new PictureProcess();

        public static int NUM_POINTS_PER_LIKE = 10;

        public PictureHelper(ApplicationDbContext context)
        {
            db = context;
        }

        public ICollection<Picture> GetAllPictures()
        {
            bool flag = db.Pictures.Any(p => p.Hidden == false);
            if (flag)
                return db.Pictures.Where(p => p.Hidden == false).ToList();
            else
                return null;
        }

        public ICollection<Picture> GetPicturesOrderedByTitle()
        {
            bool flag = db.Pictures.Any(p => p.Hidden == false);
            if (flag)
                return db.Pictures.Where(p => p.Hidden == false).OrderBy(p => p.Title).ToList();
            else
                return null;
        }

        public ICollection<Picture> GetPicturesOrderedByMostPurchased()
        {
            return db.Pictures.Where(p => p.Hidden == false).ToList().OrderByDescending(p => p.SalesCount).ThenByDescending(p => p.UploadTime).ToList();
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

            ICollection<Tag> tags = db.Tags.Where(t => t.Description.ToLower().Contains(searchString)).ToList();

            List<Picture> pictures = new List<Picture>();

            foreach (Tag t in tags)
            {
                if (t.Pictures != null)
                {
                    foreach (Picture p in t.Pictures)
                    {
                        if (!p.Hidden)
                        {
                            pictures.Add(p);
                        }
                    }
                }

            }

            return pictures;

        }

        public Tuple<Picture, bool> LikePicture(int pictureId, string userId)
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
                return Tuple.Create(picture, false);
            }


            picture.NumberOfLikes++;
            picture.LikedBy = (picture.LikedBy ?? new List<UserInfo>());

            if (!picture.LikedBy.Contains(userInfo))
            {
                picture.LikedBy.Add(userInfo);
                picture.Owner.AccountBalance = picture.Owner.AccountBalance + NUM_POINTS_PER_LIKE;
            }

            userInfo.LikedPictures = (userInfo.LikedPictures ?? new List<Picture>());
            userInfo.LikedPictures.Add(picture);
            db.SaveChanges();

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        string EntityTypeStr = "Entity of type " + eve.Entry.Entity.GetType().Name.ToString() + " in state " + eve.Entry.State.ToString() + " has the following validation errors:";
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            string PropertyStr = "Property: " + ve.PropertyName.ToString() + ", Error: " + ve.ErrorMessage.ToString();
            //        }
            //    }
            //    throw;
            //}

            return Tuple.Create(picture, true);
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

        public string[] TagSplit(string tagsStr)
        {
            string temp = "";
            foreach (var ch in tagsStr)
            {
                if (!(char.IsLetter(ch)) && !(char.IsDigit(ch)))
                    temp += " ";
                else
                    temp += ch;
            }
            return System.Text.RegularExpressions.Regex.Split(temp, @"[ ]+");
        }

        public Picture CreatPicture(string userID, string title,
            decimal cost, string location, string description,
            string tags, DateTime time, Picture.ValidFileType type,
            byte[] data)
        {
            Picture pic = new Picture();
            pic.OwnerId = userID;
            pic.Title = title;
            pic.Cost = cost;

            if (location == "")
                pic.Location = null;
            else
                pic.Location = location;

            if (description == "")
                pic.Description = null;
            else
                pic.Description = description;

            pic.UploadTime = time;
            pic.PictureType = type;
            pic.OriginalImg = data;
            pic.CompressImg = picPro.ZoomAuto(data);
            pic.Tags = new List<Tag>();
            if (tags == "")
                pic.Tags = null;
            else
            {
                string[] tagsStr = TagSplit(tags);
                foreach (var tagstr in tagsStr)
                {
                    bool flag = db.Tags.Any(emp => emp.Description == tagstr);

                    if (flag)
                    {
                        Tag tag = db.Tags.Single(emp => emp.Description == tagstr);
                        pic.Tags.Add(tag);
                    }
                    else
                    {
                        Tag tag = new Tag();
                        tag.Description = tagstr;
                        tag.Pictures = new List<Picture>();
                        pic.Tags.Add(tag);
                        tag.Pictures.Add(pic);
                    }
                }
            }
            return pic;
        }

        public ICollection<Picture> GetOwnedPictures(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            bool flag = user.OwnedPictures.Any(p => p.Hidden == false);
            if (flag)
                return user.OwnedPictures.Where(p => p.Hidden == false).ToList();
            else
                return null;
        }

        public Picture GetSpecificPicture(int id)
        {
            Picture pic = db.Pictures.Single(emp => emp.Id == id);
            return pic;
        }

        public ICollection<Picture> GetLikedPictures(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            bool flag = user.LikedPictures.Any(p => p.Hidden == false);
            if (flag)
                return user.LikedPictures.Where(p => p.Hidden == false).ToList();
            else
                return null;
        }

        public int NumPicNotHidden(List<Picture> pictures)
        {
            int num = 0;
            foreach (Picture pic in pictures)
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
