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
    public class CartHelperTest
    {

        [TestMethod]
        public void TestRemovePictureFromCart()
        {

            Cart cart = new Cart { UserId = "test_user" };
            Picture firstPicture = new Picture { Id = 1, Title = "First Picture" };
            Picture secondPicture = new Picture { Id = 2, Title = "Second Picture" };

            cart.PicturesInCart = new List<Picture> { firstPicture, secondPicture };

            Cart secondCart = new Cart { UserId = "test_user2" };
            secondCart.PicturesInCart = new List<Picture> { firstPicture };

            var cartData = new List<Cart> 
            { 
                cart,
                secondCart,
            }.AsQueryable();

            var pictureData = new List<Picture> 
            { 
                firstPicture,
                secondPicture,
            }.AsQueryable(); 

            var cartMockSet = new Mock<DbSet<Cart>>();
            cartMockSet.As<IQueryable<Cart>>().Setup(m => m.Provider).Returns(cartData.Provider);
            cartMockSet.As<IQueryable<Cart>>().Setup(m => m.Expression).Returns(cartData.Expression);
            cartMockSet.As<IQueryable<Cart>>().Setup(m => m.ElementType).Returns(cartData.ElementType);
            cartMockSet.As<IQueryable<Cart>>().Setup(m => m.GetEnumerator()).Returns(cartData.GetEnumerator());

            var pictureMockSet = new Mock<DbSet<Picture>>();
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.Provider).Returns(pictureData.Provider);
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.Expression).Returns(pictureData.Expression);
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.ElementType).Returns(pictureData.ElementType);
            pictureMockSet.As<IQueryable<Picture>>().Setup(m => m.GetEnumerator()).Returns(pictureData.GetEnumerator()); 

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(m => m.Carts).Returns(cartMockSet.Object);
            mockContext.Setup(p => p.Pictures).Returns(pictureMockSet.Object);

            var helper = new CartHelper(mockContext.Object);
            helper.RemovePictureFromCart("test_user", 1);

            ICollection<Picture> pictures = mockContext.Object.Pictures.ToList();
            Assert.AreEqual(2, pictures.Count);

            ICollection<Cart> carts = mockContext.Object.Carts.ToList();
            Assert.AreEqual(2, carts.Count);
            Assert.AreEqual(1, carts.ElementAt(0).PicturesInCart.Count);
            Assert.AreEqual(1, carts.ElementAt(1).PicturesInCart.Count);

            mockContext.Verify(m => m.SaveChanges(), Times.Once());

        }


    }
}
