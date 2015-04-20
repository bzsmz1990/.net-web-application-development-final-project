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

        [TestMethod]
        public void TestGetPicturesOrderedByMostRecent()
        {

            var data = new List<Picture> 
            { 
                new Picture { Title = "Picture1", UploadTime = new DateTime(2015, 1, 18) }, 
                new Picture { Title = "Picture2", UploadTime = new DateTime(2015, 2, 18) }, 
                new Picture { Title = "Picture3", UploadTime = new DateTime(2015, 3, 18) }, 
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Picture>>();
            mockSet.As<IQueryable<Picture>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Picture>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Picture>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Picture>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Pictures).Returns(mockSet.Object);

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
            ICollection<Transaction> firstTransactions = new List<Transaction>();
            ICollection<Transaction> secondTransactions = new List<Transaction>();
            ICollection<Transaction> thirdTransactions = new List<Transaction>();

            secondTransactions.Add(new Transaction());
            secondTransactions.Add(new Transaction());
            thirdTransactions.Add(new Transaction());

            var data = new List<Picture> 
            { 
                new Picture { Title = "Picture1", SaleTransactions = firstTransactions }, 
                new Picture { Title = "Picture2", SaleTransactions = secondTransactions }, 
                new Picture { Title = "Picture3", SaleTransactions = thirdTransactions }, 
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Picture>>();
            mockSet.As<IQueryable<Picture>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Picture>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Picture>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Picture>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Pictures).Returns(mockSet.Object);

            var helper = new PictureHelper(mockContext.Object);
            var pictures = helper.GetPicturesOrderedByMostPurchased();

            Assert.AreEqual(3, pictures.Count);
            Assert.AreEqual("Picture2", pictures.ElementAt(0).Title);
            Assert.AreEqual("Picture3", pictures.ElementAt(1).Title);
            Assert.AreEqual("Picture1", pictures.ElementAt(2).Title);

        }


    }
}
