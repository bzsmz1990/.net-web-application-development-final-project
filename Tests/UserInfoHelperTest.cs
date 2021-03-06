﻿using System;
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
    public class UserInfoHelperTest
    {
        private IQueryable<UserInfo> userData;
        private Mock<DbSet<UserInfo>> userMockSet;
        private Mock<ApplicationDbContext> mockContext;


        [TestInitialize()]
        public void Initialize()
        {

            UserInfo user1 = new UserInfo { UserId = "User1" };
            UserInfo user2 = new UserInfo { UserId = "User2" };
            UserInfo user3 = new UserInfo { UserId = "User3" };


            userData = new List<UserInfo>
            {
                user1, user2, user3
            }.AsQueryable();

            userMockSet = new Mock<DbSet<UserInfo>>();
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.Provider).Returns(userData.Provider);
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.Expression).Returns(userData.Expression);
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            userMockSet.As<IQueryable<UserInfo>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.UserInfos).Returns(userMockSet.Object);


        }

        [TestMethod]
        public void TestFollowUser()
        {
            var helper = new UserInfoHelper(mockContext.Object);

            Assert.AreEqual(0, userData.ElementAt(0).AccountBalance);
            Assert.AreEqual(0, userData.ElementAt(1).AccountBalance);

            Tuple<UserInfo, bool> followingUser = helper.FollowUser(userData.ElementAt(0).UserId, userData.ElementAt(1).UserId);

            Assert.IsNotNull(followingUser.Item1);
            Assert.AreEqual(1, userData.ElementAt(0).Following.Count);
            Assert.AreEqual(1, userData.ElementAt(1).Followers.Count);
            Assert.AreEqual(UserInfoHelper.NUM_POINTS_PER_FOLLOW, userData.ElementAt(1).AccountBalance);

            followingUser = helper.FollowUser(userData.ElementAt(2).UserId, userData.ElementAt(1).UserId);

            Assert.IsNotNull(followingUser.Item1);
            Assert.AreEqual(1, userData.ElementAt(2).Following.Count);
            Assert.AreEqual(2, userData.ElementAt(1).Followers.Count);
            Assert.AreEqual(UserInfoHelper.NUM_POINTS_PER_FOLLOW * 2, userData.ElementAt(1).AccountBalance);

            helper.FollowUser(userData.ElementAt(0).UserId, userData.ElementAt(1).UserId);

            Assert.IsNotNull(followingUser.Item1);
            Assert.AreEqual(0, userData.ElementAt(0).Following.Count);
            Assert.AreEqual(1, userData.ElementAt(1).Followers.Count);
            Assert.AreEqual(UserInfoHelper.NUM_POINTS_PER_FOLLOW, userData.ElementAt(1).AccountBalance);

        }

        [TestMethod]
        public void TestGetUser()
        {
            var helper = new UserInfoHelper(mockContext.Object);
            string userId = userData.ElementAt(0).UserId;
            var temp = helper.GetUser(userId);
            Assert.AreEqual("DataLayer.UserInfo", temp.GetType().ToString());
            Assert.AreEqual("User1", temp.UserId);
        }

        [TestMethod]
        public void TestSetLevel()
        {
            UserInfo info = userData.ElementAt(0);
            var helper = new UserInfoHelper(mockContext.Object);

            Assert.AreEqual(1, info.Level);
            helper.SetLevel(info);
            Assert.AreEqual(1, info.Level);

            info.SaleTransactions = new List<Transaction>()
            {
                new Transaction(), new Transaction(), new Transaction(), new Transaction(), new Transaction(), 
            };

            helper.SetLevel(info);
            Assert.AreEqual(2, info.Level);

            info.SaleTransactions = new List<Transaction>()
            {
                new Transaction(), new Transaction(), new Transaction(), new Transaction(), new Transaction(), 
                new Transaction(), new Transaction(), new Transaction(), new Transaction(), new Transaction(), 
            };

            helper.SetLevel(info);
            Assert.AreEqual(3, info.Level);

            info.SaleTransactions = new List<Transaction>()
            {
                new Transaction(), new Transaction(), new Transaction(), new Transaction(), new Transaction(), 
                new Transaction(), new Transaction(), new Transaction(), new Transaction(), new Transaction(), 
                new Transaction(), new Transaction(), new Transaction(), new Transaction(), new Transaction(), 
            };

            helper.SetLevel(info);
            Assert.AreEqual(4, info.Level);

        }
    }
}
