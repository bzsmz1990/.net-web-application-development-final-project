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
        public static Tuple<ApplicationUser, UserInfo> CreateNewUser(string userName, string email, string firstName, string lastName) //TODO: ADD FIRST AND LAST NAME -- DONE (BY DIEGO)
        {
            UserInfo userInfo = new UserInfo {FirstName=firstName, LastName=lastName};
            ApplicationUser appUser = new ApplicationUser {FirstName = firstName, LastName = lastName, UserName = userName, Email = email, Info = userInfo };
            return Tuple.Create(appUser, userInfo);

        }

        public static UserInfo CreateNewUserInfo(string userName, string email, string firstName, string lastName, string id) //TODO: ADD FIRST AND LAST NAME -- DONE (BY DIEGO)
        {
            UserInfo userInfo = new UserInfo {UserId=id, FirstName = firstName, LastName = lastName };
            return userInfo;
        }
    }
}
