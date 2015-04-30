using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Business_Logic;

namespace PhotoProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private UserInfoHelper userHelp = new UserInfoHelper(db);

        // GET: Cart
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.User);
            if (carts == null)
            {
                return View(new List<Picture>());
            }

            Cart cart = carts.FirstOrDefault();

            bool isMoreExpensive = false;
            if (cart.AlbumsInCart != null)
            {
                foreach (Album al in cart.AlbumsInCart)
                {
                    if (!CartHelper.checkIfAlbumIsMoreExpensive(al))
                    {
                        isMoreExpensive = true;
                        break;
                    }
                }
            }
            if (isMoreExpensive)
            {
                ViewBag.AlbumFlag = true;
            }
            else
            {
                ViewBag.AlbumFlag = false;
            }
            return View(carts.ToList());
        }

        public ActionResult CheckOut()
        {
       
            UserInfo user = db.UserInfos.Include(c => c.User).FirstOrDefault();
            Cart cart = user.Cart;
            if (cart.PicturesInCart == null && cart.AlbumsInCart == null)
            {
                ViewBag.Message = "No items in cart";
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.CartTotal = CartHelper.getTotalFromCart(cart);
            ViewBag.Pictures = cart.PicturesInCart.ToList();
            ViewBag.Albums = cart.AlbumsInCart.ToList();

            return View(user);
        }

        public ActionResult Submit()
        {           
            UserInfo user = db.UserInfos.Include(c => c.User).FirstOrDefault();
            List<Transaction> picTrans = CartHelper.generatePictureTransactions(user);
            List<Transaction> albumTrans = CartHelper.generateAlbumTransactions(user);

            //Put album and picture transactions in one list
            List<Transaction> AllTransactions = CartHelper.mergeLists(picTrans, albumTrans);
            foreach (Transaction trans in albumTrans)
            {
                AllTransactions.Add(trans);
            }

            //Update Account Balances and add Transactions to the db
            foreach(Transaction trans in AllTransactions) {
                decimal total = trans.TotalAmount;
                user.AccountBalance -= total;
                trans.Seller.AccountBalance += total;
                db.Transactions.Add(trans);
                db.SaveChanges();
                userHelp.SetLevel(trans.Seller);
            }

            return View();


        }

        public ActionResult BuyPicture(int picId)
        {
            UserInfo user = db.UserInfos.Include(c => c.User).FirstOrDefault();
            user.Cart = (user.Cart ?? new Cart());
            Cart cart = user.Cart;
            CartHelper helper = new CartHelper(db);
            if (user.OwnedPictures != null)
            {
                foreach (Picture pic in user.OwnedPictures)
                {
                    if (pic.Id == picId)
                    {
                        ViewBag.Error = "Already owned picture";
                        //PERHAPS REDIRECT TO SOMEPLACE ELSE
                        return RedirectToAction("Index", "UserHome");
                    }
                }
            }
            if (helper.buyPicture(picId, cart.UserId) == false)
            {
                //TODO: REDIRECT TO ERROR
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult BuyAlbum(int albumId)
        {
            UserInfo user = db.UserInfos.Include(c => c.User).FirstOrDefault();
            user.Cart = (user.Cart ?? new Cart());
            Cart cart = user.Cart;
            CartHelper helper = new CartHelper(db);
            if (!helper.buyAlbum(albumId, cart.UserId))
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Cart");
        }

        // GET: Cart/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserInfos, "UserId", "FirstName");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserInfos, "UserId", "FirstName", cart.UserId);
            return View(cart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserInfos, "UserId", "FirstName", cart.UserId);
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserInfos, "UserId", "FirstName", cart.UserId);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
