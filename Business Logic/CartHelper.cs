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
            decimal total = 0;
            foreach (Picture pic in cart.PicturesInCart)
            {
                total += pic.Cost;
            }
            foreach (Album alb in cart.AlbumsInCart)
            {
                total += alb.Cost;
            }
            return total;
        }

    }
}
