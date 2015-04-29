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

        public static List<Transaction> generatePictureTransactions(UserInfo user)
        {
            
            Cart cart = user.Cart;
            if (cart == null)
            {
                throw new NullReferenceException();
            }
            if (cart.PicturesInCart == null)
            {
                return null;
            }

            List<Transaction> transactions = new List<Transaction>();
            List<Picture> pics = cart.PicturesInCart.ToList();
            for (int i = 0; i < cart.PicturesInCart.Count; i++)
            {
                Transaction transaction = new Transaction
                {
                    Buyer = user,
                    Seller = pics[i].Owner,
                    TotalAmount = pics[i].Cost,
                    PicturesBeingSold = new List<Picture>() { pics[i] }
                };
                transactions.Add(transaction);
            }

            for (int i = 0; i < transactions.Count; i++)
            {
                Transaction t = transactions[i];
                string seller = t.Seller.UserId;
                for (int j = i + 1; j < transactions.Count; j++)
                {
                    Transaction tr = transactions[j];
                    string otherseller = tr.Seller.UserId;
                    if (seller.Equals(otherseller))
                    {
                        t.PicturesBeingSold.Add(tr.PicturesBeingSold.FirstOrDefault());
                        t.TotalAmount += tr.TotalAmount;
                        transactions.RemoveAt(j);
                    }
                }
            }

            

            return transactions;
        }

        public static List<Transaction> generateAlbumTransactions(UserInfo user)
        {
            Cart cart = user.Cart;
            if (cart.AlbumsInCart == null)
            {
                return null;
            }

            if (cart == null)
            {
                throw new NullReferenceException();
            }

            List<Transaction> transactions = new List<Transaction>();
            List<Album> albums = cart.AlbumsInCart.ToList();
            for (int i = 0; i < cart.AlbumsInCart.Count; i++)
            {
                Transaction transaction = new Transaction
                {
                    Buyer = user,
                    Seller = albums[i].User,
                    TotalAmount = albums[i].Cost,
                    AlbumsBeingSold = new List<Album>() { albums[i] }
                };
                transactions.Add(transaction);
            }

            for (int i = 0; i < transactions.Count; i++)
            {
                Transaction t = transactions[i];
                string seller = t.Seller.UserId;
                for (int j = i + 1; j < transactions.Count; j++)
                {
                    Transaction tr = transactions[j];
                    string otherseller = tr.Seller.UserId;
                    if (seller.Equals(otherseller))
                    {
                        t.AlbumsBeingSold.Add(tr.AlbumsBeingSold.FirstOrDefault());
                        t.TotalAmount += tr.TotalAmount;
                        transactions.RemoveAt(j);
                    }
                }
            }
            return transactions;
        }

        public static List<Transaction> mergeLists(List<Transaction> picTrans, List<Transaction> albumTrans)
        {
            //Merge picTrans and AlbumTrans
            for (int i = 0; i < picTrans.Count; i++)
            {
                Transaction t = picTrans[i];
                string seller = t.Seller.UserId;
                for (int j = 0; j < albumTrans.Count; j++)
                {
                    Transaction albumTr = albumTrans[j];
                    string otherSeller = albumTr.Seller.UserId;
                    if (seller.Equals(otherSeller))
                    {
                        t.AlbumsBeingSold = (t.AlbumsBeingSold ?? new List<Album>());
                        foreach (Album al in albumTr.AlbumsBeingSold)
                        {
                            t.AlbumsBeingSold.Add(al);
                        }
                        t.TotalAmount += albumTr.TotalAmount;
                        albumTrans.RemoveAt(j);
                    }
                }
            }
            List<Transaction> allTrans = new List<Transaction>();
            foreach (Transaction trans in picTrans)
            {
                allTrans.Add(trans);
            }
            foreach (Transaction trans in albumTrans)
            {
                allTrans.Add(trans);
            }
            return allTrans;
        }

    }
}
