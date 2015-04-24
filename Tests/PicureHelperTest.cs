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
        private Mock<DbSet<Picture>> pictureMockSet;
        private Mock<DbSet<Tag>> tagsMockSet;
        private Mock<ApplicationDbContext> mockContext;


        [TestInitialize()]
        public void Initialize()
        {
            Tag tag1 = new Tag { Description = "NYU" };
            Tag tag2 = new Tag { Description = "Computer Science" };

            
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

            Picture picture1 = new Picture { Title = "Picture1", UploadTime = new DateTime(2015, 1, 18), Tags = new List<Tag>{ tag1 } };
            Picture picture2 = new Picture { Title = "Picture2", UploadTime = new DateTime(2015, 2, 18), SaleTransactions = secondTransactions, Tags = new List<Tag> { tag1, tag2 } };
            Picture picture3 = new Picture { Title = "Picture3", UploadTime = new DateTime(2015, 3, 18), SaleTransactions = thirdTransactions };

            tag1.Pictures = new List<Picture> { picture1, picture2 };
            tag2.Pictures = new List<Picture> { picture2 };

            pictureData = new List<Picture> 
            { 
                picture1, 
                picture2, 
                picture3, 
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

            mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Pictures).Returns(pictureMockSet.Object);
            mockContext.Setup(c => c.Tags).Returns(tagsMockSet.Object);


        }

        [TestMethod]
        public void TestGetPicturesOrderedByMostRecent()
        {
            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesOrderedByMostRecent();

            Assert.AreEqual(3, pictures.Count);
            Assert.AreEqual("Picture3", pictures.ElementAt(0).Title);
            Assert.AreEqual("Picture2", pictures.ElementAt(1).Title);
            Assert.AreEqual("Picture1", pictures.ElementAt(2).Title); 

        }

        [TestMethod]
        public void TestGetPicturesOrderedByMostPurchased()
        {

            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesOrderedByMostPurchased();

            Assert.AreEqual(3, pictures.Count);
            Assert.AreEqual("Picture2", pictures.ElementAt(0).Title);
            Assert.AreEqual("Picture3", pictures.ElementAt(1).Title);
            Assert.AreEqual("Picture1", pictures.ElementAt(2).Title);

        }

        [TestMethod]
        public void TestGetAllPictures()
        {
            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetAllPictures();
            Assert.AreEqual(3, pictures.Count);
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
            Assert.AreEqual(3, pictures.Count);

            pictures = helper.GetPicturesWhereTagHasWord("");
            Assert.AreEqual(3, pictures.Count);

            pictures = helper.GetPicturesWhereTagHasWord("    ");
            Assert.AreEqual(3, pictures.Count);

        }


    }
}
