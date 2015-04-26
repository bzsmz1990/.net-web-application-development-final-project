using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using Business_Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq; 

namespace Tests
{
    [TestClass]
    public class PicureHelperTest
    {
        private IQueryable<Picture> pictureData;
        private IQueryable<Tag> tagsData;
        private IQueryable<UserInfo> userData;
        private Mock<DbSet<Picture>> pictureMockSet;
        private Mock<DbSet<Tag>> tagsMockSet;
        private Mock<DbSet<UserInfo>> userMockSet;
        private Mock<ApplicationDbContext> mockContext;


        [TestInitialize()]
        public void Initialize()
        {
            Tag tag1 = new Tag { Description = "NYU" };
            Tag tag2 = new Tag { Description = "Computer Science" };

            UserInfo user1 = new UserInfo { UserId = "User1" };
            UserInfo user2 = new UserInfo { UserId = "User2" };
            UserInfo user3 = new UserInfo { UserId = "User3" };
            UserInfo user4 = new UserInfo { UserId = "User4" };
            UserInfo user5 = new UserInfo { UserId = "User5" };

            
            ICollection<Transaction> secondTransactions = new List<Transaction>
            {
                new Transaction(),
                new Transaction(),
                new Transaction()
            };
            ICollection<Transaction> thirdTransactions = new List<Transaction>
            {
                new Transaction(),
                new Transaction()
            };

            Picture picture1 = new Picture { Id = 1, Title = "Picture1", UploadTime = new DateTime(2015, 1, 18), Tags = new List<Tag>{ tag1 } };
            Picture picture2 = new Picture { Id = 2, Title = "Picture2", UploadTime = new DateTime(2015, 2, 18), SaleTransactions = secondTransactions, Tags = new List<Tag> { tag1, tag2 } };
            Picture picture3 = new Picture { Id = 3, Title = "Picture3", UploadTime = new DateTime(2015, 3, 18), SaleTransactions = thirdTransactions };
            Picture picture4 = new Picture { Id = 4, Title = "Picture4", UploadTime = new DateTime(2015, 4, 18)};
            Picture picture5 = new Picture { Id = 5, Title = "Picture5", UploadTime = new DateTime(2015, 5, 18) };

            user4.OwnedPictures = new List<Picture> { picture4, picture5 };
            user5.LikedPictures = new List<Picture> { picture4, picture5 };

            tag1.Pictures = new List<Picture> { picture1, picture2 };
            tag2.Pictures = new List<Picture> { picture2 };

            userData = new List<UserInfo>
            {
                user1, user2, user3,user4,user5,
            }.AsQueryable();

            pictureData = new List<Picture> 
            { 
                picture1, 
                picture2, 
                picture3, 
                picture4,
                picture5,
            }.AsQueryable();

            tagsData = new List<Tag> 
            { 
                tag1, 
                tag2, 
            }.AsQueryable();

            pictureMockSet = new Mock<DbSet<Picture>>();
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.Provider).Returns(pictureData.Provider);
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.Expression).Returns(pictureData.Expression);
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.ElementType).Returns(pictureData.ElementType);
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.GetEnumerator()).Returns(pictureData.GetEnumerator());

            tagsMockSet = new Mock<DbSet<Tag>>();
            tagsMockSet.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(tagsData.Provider);
            tagsMockSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(tagsData.Expression);
            tagsMockSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(tagsData.ElementType);
            tagsMockSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(tagsData.GetEnumerator());

            userMockSet = new Mock<DbSet<UserInfo>>();
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.Provider).Returns(userData.Provider);
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.Expression).Returns(userData.Expression);
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());


            mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Pictures).Returns(pictureMockSet.Object);
            mockContext.Setup(c => c.Tags).Returns(tagsMockSet.Object);
            mockContext.Setup(c => c.UserInfos).Returns(userMockSet.Object);


        }

        [TestMethod]
        public void TestGetPicturesOrderedByMostRecent()
        {
            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesOrderedByMostRecent();

            Assert.AreEqual(5, pictures.Count);
            Assert.AreEqual("Picture5", pictures.ElementAt(0).Title);
            Assert.AreEqual("Picture4", pictures.ElementAt(1).Title);
            Assert.AreEqual("Picture3", pictures.ElementAt(2).Title);

        }

        [TestMethod]
        public void TestGetPicturesOrderedByMostPurchased()
        {

            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesOrderedByMostPurchased();

            Assert.AreEqual(5, pictures.Count);
            Assert.AreEqual("Picture2", pictures.ElementAt(0).Title);
            Assert.AreEqual("Picture3", pictures.ElementAt(1).Title);
            Assert.AreEqual("Picture5", pictures.ElementAt(2).Title);

        }

        [TestMethod]
        public void TestGetAllPictures()
        {
            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetAllPictures();
            Assert.AreEqual(5, pictures.Count);
        }

        [TestMethod]
        public void TestGetPicturesWhereTagHasWordWithValidWord()
        {
            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesWhereTagHasWord("NYU");
            Assert.AreEqual(2, pictures.Count);
            Assert.AreEqual("Picture1", pictures.ElementAt(0).Title);
            Assert.AreEqual("Picture2", pictures.ElementAt(1).Title);

            pictures = helper.GetPicturesWhereTagHasWord("computer");
            Assert.AreEqual(1, pictures.Count);
            Assert.AreEqual("Picture2", pictures.ElementAt(0).Title);
        }

        [TestMethod]
        public void TestGetPicturesWhereTagHasWordWithInValidWord()
        {
            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesWhereTagHasWord(null);
            Assert.AreEqual(5, pictures.Count);

            pictures = helper.GetPicturesWhereTagHasWord("");
            Assert.AreEqual(5, pictures.Count);

            pictures = helper.GetPicturesWhereTagHasWord("    ");
            Assert.AreEqual(5, pictures.Count);

        }
        [TestMethod]
        public void TestLikePicture()
        {
            var helper = new PictureHelper(mockContext.Object);
            int pictureId = pictureData.ElementAt(0).Id;
            string userId = userData.ElementAt(0).UserId;

            Assert.AreEqual(0, pictureData.ElementAt(0).NumberOfLikes);
            Assert.IsNull(userData.ElementAt(0).LikedPictures);

            Picture likedPicture = helper.LikePicture(pictureId, userId);

            Assert.IsNotNull(likedPicture);
            Assert.AreEqual(1, pictureData.ElementAt(0).NumberOfLikes);
            Assert.AreEqual(1, userData.ElementAt(0).LikedPictures.Count);
            Assert.AreEqual(1, pictureData.ElementAt(0).LikedBy.Count);

            likedPicture = helper.LikePicture(pictureId, userData.ElementAt(1).UserId);
            Assert.IsNotNull(likedPicture);
            Assert.AreEqual(2, pictureData.ElementAt(0).NumberOfLikes);
            Assert.AreEqual(1, userData.ElementAt(1).LikedPictures.Count);
            Assert.AreEqual(2, pictureData.ElementAt(0).LikedBy.Count);

            likedPicture = helper.LikePicture(pictureId, userId);
            Assert.IsNotNull(likedPicture);
            Assert.AreEqual(1, pictureData.ElementAt(0).NumberOfLikes);
            Assert.AreEqual(0, userData.ElementAt(0).LikedPictures.Count);
            Assert.AreEqual(2, pictureData.ElementAt(0).LikedBy.Count);

        }


        [TestMethod]
        public void TestReportPicture()
        {
            var helper = new PictureHelper(mockContext.Object);
            int pictureId = pictureData.ElementAt(0).Id;

            Assert.IsFalse(pictureData.ElementAt(0).HasBeenReported);
            Picture changedPicture = helper.ReportPicture(pictureData.ElementAt(0).Id);
            Assert.IsNotNull(changedPicture);
            Assert.IsTrue(pictureData.ElementAt(0).HasBeenReported);

        }

        [TestMethod]
        public void TestCreatPicture()
        {
            var helper = new PictureHelper(mockContext.Object);
            Picture pic = helper.CreatPicture("123", "testPi", (decimal)1.0, "NYU", "It's a test", DateTime.MinValue, Picture.ValidFileType.jpg, null);
            Assert.IsNotNull(pic);
            Assert.AreEqual("123", pic.OwnerId);
            Assert.AreEqual("testPi", pic.Title);
            Assert.AreEqual((decimal)1.0, pic.Cost);
            Assert.AreEqual(DateTime.MinValue, pic.UploadTime);
            Assert.AreEqual(Picture.ValidFileType.jpg, pic.PictureType);
            Assert.IsNull(pic.OriginalImg);
            Assert.IsNull(pic.CompressImg);
        }

        [TestMethod]
        public void TestGetOwnedPictures()
        {
            var helper = new PictureHelper(mockContext.Object);
            string userId = userData.ElementAt(3).UserId;
            Assert.IsNotNull(userId);
            List<Picture> ownPics = helper.GetOwnedPictures(userId).ToList();
            Assert.AreEqual(2, ownPics.Count());
            Assert.AreEqual(4, ownPics[0].Id);
            Assert.AreEqual("Picture5", ownPics[1].Title);
            Assert.AreEqual(new DateTime(2015, 5, 18), ownPics[1].UploadTime);
        }

        [TestMethod]
        public void TestGetLikedPictures()
        {
            var helper = new PictureHelper(mockContext.Object);
            string userId = userData.ElementAt(4).UserId;
            Assert.IsNotNull(userId);
            List<Picture> likePics = helper.GetLikedPictures(userId).ToList();
            Assert.AreEqual(2, likePics.Count());
            Assert.AreEqual(4, likePics[0].Id);
            Assert.AreEqual("Picture5", likePics[1].Title);
            Assert.AreEqual(new DateTime(2015, 5, 18), likePics[1].UploadTime);
        }

        [TestMethod]
        public void TestNumPicNotHidden()
        {
            var helper = new PictureHelper(mockContext.Object);
            Picture pic1 = new Picture { Id = 1, Title = "Picture1", Hidden = false };
            Picture pic2 = new Picture { Id = 2, Title = "Picture2", Hidden = false };
            Picture pic3 = new Picture { Id = 3, Title = "Picture3", Hidden = true };
            List<Picture> pics = new List<Picture> { pic1, pic2, pic3 };
            int num = helper.NumPicNotHidden(pics);
            Assert.AreEqual(2, num);
        }

        [TestMethod]
        public void TestVerifyUserLevel()
        {
            var helper = new PictureHelper(mockContext.Object);

            UserInfo user1 = new UserInfo { UserId = "User1",Level=1 };
            UserInfo user2 = new UserInfo { UserId = "User2",Level=1 };
            UserInfo user3 = new UserInfo { UserId = "User3",Level=2 };
            UserInfo user4 = new UserInfo { UserId = "User4",Level=2 };
            UserInfo user5 = new UserInfo { UserId = "User5",Level=3 };
            UserInfo user6 = new UserInfo { UserId = "User6",Level=3 };

            List<Picture> pics1 = new List<Picture>();
            for(int i=1;i<=5;i++)
            {
                Picture pic = new Picture { Id = i, Title="Pic"+i };
                pics1.Add(pic);
            }
            user1.OwnedPictures = pics1;
            bool status1 = helper.VerifyUserLevel(user1);
            Assert.AreEqual(true, status1);

            List<Picture> pics2 = new List<Picture>();
            for (int i = 1; i <= 11; i++)
            {
                Picture pic = new Picture { Id = i, Title = "Pic" + i };
                pics2.Add(pic);
            }
            user2.OwnedPictures = pics2;
            bool status2 = helper.VerifyUserLevel(user2);
            Assert.AreEqual(false, status2);

            List<Picture> pics3 = new List<Picture>();
            for (int i = 1; i <= 15; i++)
            {
                Picture pic = new Picture { Id = i, Title = "Pic" + i };
                pics3.Add(pic);
            }
            user3.OwnedPictures = pics3;
            bool status3 = helper.VerifyUserLevel(user3);
            Assert.AreEqual(true, status3);

            List<Picture> pics4 = new List<Picture>();
            for (int i = 1; i <= 20; i++)
            {
                Picture pic = new Picture { Id = i, Title = "Pic" + i };
                pics4.Add(pic);
            }
            user4.OwnedPictures = pics4;
            bool status4 = helper.VerifyUserLevel(user4);
            Assert.AreEqual(false, status4);

            List<Picture> pics5 = new List<Picture>();
            for (int i = 1; i <= 25; i++)
            {
                Picture pic = new Picture { Id = i, Title = "Pic" + i };
                pics5.Add(pic);
            }
            user5.OwnedPictures = pics5;
            bool status5 = helper.VerifyUserLevel(user5);
            Assert.AreEqual(true, status5);

            List<Picture> pics6 = new List<Picture>();
            for (int i = 1; i <= 35; i++)
            {
                Picture pic = new Picture { Id = i, Title = "Pic" + i };
                pics6.Add(pic);
            }
            user6.OwnedPictures = pics6;
            bool status6 = helper.VerifyUserLevel(user6);
            Assert.AreEqual(false, status6);
        }
    }
}
