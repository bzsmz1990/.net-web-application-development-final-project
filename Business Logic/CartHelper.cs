using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class CartHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CartHelper(ApplicationDbContext context)
        {
            db = context;

        }

        //TODO: THIS SHOULD CHECK FOR NULL CART OR PICTURE
        //OR REECEIVE THE CART / PICTURE AS INPUT
        public void RemovePictureFromCart(string cartId, int pictureId)
        {
            Cart cart = db.Carts.SingleOrDefault(c => c.UserId == cartId);
            Picture picture = db.Pictures.SingleOrDefault(p => p.Id == pictureId);
            cart.PicturesInCart.Remove(picture);
            db.SaveChanges();
        }

        public static decimal getTotalFromCart(Cart cart)
        {
            if (cart == null)
            {
                throw new NullReferenceException();
            }
            decimal total = 0;
            if (cart.PicturesInCart != null)
            {
                foreach (Picture pic in cart.PicturesInCart)
                {
                    total += pic.Cost;
                }
            }
            if (cart.AlbumsInCart != null)
            {
                foreach (Album alb in cart.AlbumsInCart)
                {
                    total += alb.Cost;
                }
            }
            return total;
        }

        public static Boolean checkIfAlbumIsMoreExpensive(Album al)
        {
            decimal sum = 0;
            foreach (Picture pic in al.Pictures)
            {
                sum += pic.Cost;
            }
            if (sum <= al.Cost)
            {
                return true;
            }
            return false;
        }

        public Boolean buyPicture(int picId, string cartId)
        {
            if (cartId == null)
            {
                return false;
            }
            Cart cart = db.Carts.SingleOrDefault(c => c.UserId == cartId);
            Picture picture = db.Pictures.SingleOrDefault(p => p.Id == picId);
            if (picture == null || cart == null)
            {
                return false;
            }
            
            cart.PicturesInCart = (cart.PicturesInCart ?? new List<Picture>());
            cart.PicturesInCart.Add(picture);
            db.SaveChanges();
            return true;
        }

        public Boolean buyAlbum(int albumId, string userId) {
            if (userId == null)
            {
                return false;
            }
            UserInfo user = db.UserInfos.SingleOrDefault(c => c.UserId == userId);
            Album album = db.Albums.SingleOrDefault(a => a.Id == albumId);
            if (album == null || user == null)
            {
                return false;
            }
            user.Cart = (user.Cart ?? new Cart());
            user.Cart.AlbumsInCart = (user.Cart.AlbumsInCart ?? new List<Album>());
            user.Cart.AlbumsInCart.Add(album);
            db.SaveChanges();
            return true;
        }

    }
}
