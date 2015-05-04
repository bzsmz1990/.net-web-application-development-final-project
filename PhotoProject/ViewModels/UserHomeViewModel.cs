using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoProject.ViewModels
{
    public class GalleryViewModel
    {
        public ICollection<Picture> OwnedPictures { get; set; }
        public ICollection<Picture> LikedPictures { get; set; }
        public ICollection<UserInfo> Following { get; set; }
        public UserInfo Owner { get; set; }
        public bool isOwner { get; set; }
    }
}