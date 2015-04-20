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
        private ApplicationDbContext db { get; set; }

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
    }
}
