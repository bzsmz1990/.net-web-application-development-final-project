using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace PhotoProject.ViewModels
{
    public class CreateAlbumViewModel
    {
        public UserInfo owner { get; set; }
        public Album album { get; set; }
    }
}