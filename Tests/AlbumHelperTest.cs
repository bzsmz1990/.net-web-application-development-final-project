using Business_Logic;
using DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class AlbumHelperTest
    {
        private IQueryable<Album> albumData;
        private Mock<DbSet<Album>> albumMockSet;
        private Mock<ApplicationDbContext> albumMockContext;


        [TestInitialize()]
        public void Initialize()
        {
            ICollection<Transaction> firstTransactions = new List<Transaction>();
            ICollection<Transaction> secondTransactions = new List<Transaction>();
            ICollection<Transaction> thirdTransactions = new List<Transaction>();

            secondTransactions.Add(new Transaction());
            secondTransactions.Add(new Transaction());
            thirdTransactions.Add(new Transaction());

            albumData = new List<Album> 
            { 
                new Album { Name = "Album1", UploadTime = new DateTime(2015, 1, 18), SaleTransactions = firstTransactions }, 
                new Album { Name = "Album2", UploadTime = new DateTime(2015, 2, 18), SaleTransactions = secondTransactions }, 
                new Album { Name = "Album3", UploadTime = new DateTime(2015, 3, 18), SaleTransactions = thirdTransactions }, 
            }.AsQueryable();

            albumMockSet = new Mock<DbSet<Album>>();
            albumMockSet.As<IQueryable<Album>>().Setup(m => m.Provider).Returns(albumData.Provider);
            albumMockSet.As<IQueryable<Album>>().Setup(m => m.Expression).Returns(albumData.Expression);
            albumMockSet.As<IQueryable<Album>>().Setup(m => m.ElementType).Returns(albumData.ElementType);
            albumMockSet.As<IQueryable<Album>>().Setup(m => m.GetEnumerator()).Returns(albumData.GetEnumerator());

            albumMockContext = new Mock<ApplicationDbContext>();
            albumMockContext.Setup(c => c.Albums).Returns(albumMockSet.Object);


        }

        [TestMethod]
        public void TestGetAlbumsOrderedByMostRecent()
        {
            var helper = new AlbumHelper(albumMockContext.Object);
            var albums = helper.GetAlbumsOrderedByMostRecent();

            Assert.AreEqual(3, albums.Count);
            Assert.AreEqual("Album3", albums.ElementAt(0).Name);
            Assert.AreEqual("Album2", albums.ElementAt(1).Name);
            Assert.AreEqual("Album1", albums.ElementAt(2).Name);

        }

        [TestMethod]
        public void TestGetAlbumsOrderedByMostPurchased()
        {

            var helper = new AlbumHelper(albumMockContext.Object);
            var albums = helper.GetAlbumsOrderedByMostPurchased();

            Assert.AreEqual(3, albums.Count);
            Assert.AreEqual("Album2", albums.ElementAt(0).Name);
            Assert.AreEqual("Album3", albums.ElementAt(1).Name);
            Assert.AreEqual("Album1", albums.ElementAt(2).Name);

        }

        [TestMethod]
        public void TestGetAllAlbums()
        {
            var helper = new AlbumHelper(albumMockContext.Object);
            var albums = helper.GetAllAlbums();
            Assert.AreEqual(3, albums.Count);
        }

    }
}
