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
        public static ApplicationUser CreateNewUser(string userName, string email) //TODO: ADD FIRST AND LAST NAME
        {
            UserInfo userInfo = new UserInfo { FirstName = "Test Name", LastName = "Test Last Name" };
            return new ApplicationUser { UserName = userName, Email = email, Info = userInfo };

        }
    }
}
