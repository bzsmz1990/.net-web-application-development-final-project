using DataLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class UserInfoHelper
    {

        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserInfoHelper userHelp = new UserInfoHelper(db);

        public static int NUM_POINTS_PER_FOLLOW = 20;

        public static readonly Dictionary<int, int> UserLevelToPictureLimit
            = new Dictionary<int, int>
            {
                { 1, 10 },
                { 2, 20 },
                { 3, 30 }
            };

        
        public UserInfoHelper(ApplicationDbContext context)
        {
            db = context;
        }

        public static ApplicationUser CreateNewUser(string userName, string email) //TODO: ADD FIRST AND LAST NAME
        {
            UserInfo userInfo = new UserInfo { FirstName = "Test Name", LastName = "Test Last Name" };
            return new ApplicationUser { UserName = userName, Email = email, Info = userInfo };

        }
        public UserInfo GetUser(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            return user;
        }

        public ICollection<UserInfo> GetFollowing(string userID)
        {
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            return user.Following.ToList();
        }

        public UserInfo FollowUser(string currentUserId, string followingUserId)
        {
            UserInfo currentUser = db.UserInfos.Single(u => u.UserId == currentUserId);
            UserInfo followingUser = db.UserInfos.Single(u => u.UserId == followingUserId);

            if (currentUser == null || followingUser == null)
            {
                return null;
            }

            currentUser.Following = (currentUser.Following ?? new List<UserInfo>());

            if (currentUser.Following.Contains(followingUser))
            {
                currentUser.Following.Remove(followingUser);
                return followingUser;
            }

            currentUser.Following.Add(followingUser);

            if (followingUser.Followers == null || !followingUser.Followers.Contains(currentUser))
            {
                followingUser.AccountBalance += NUM_POINTS_PER_FOLLOW;
                followingUser.Followers = (followingUser.Followers ?? new List<UserInfo>());
                followingUser.Followers.Add(currentUser);
            }

            return followingUser;
        } 

        //TODO: NEEDS TO BE CALLED FROM THE CHECKOUT
        public void SetLevel (UserInfo user)
        {
            int soldPictures = (user.SaleTransactions == null) ? 0 : user.SaleTransactions.Count;
            if (soldPictures > UserLevelToPictureLimit[3])
            {
                user.Level = 4;
            }
            else if (soldPictures > UserLevelToPictureLimit[2])
            {
                user.Level = 3;
            }
            else if (soldPictures > UserLevelToPictureLimit[1])
            {
                user.Level = 2;
            }

            db.SaveChanges();
        }
    }
}
