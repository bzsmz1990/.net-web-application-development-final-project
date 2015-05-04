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
using System.Text;
using SendGrid;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

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
            var userID = User.Identity.GetUserId();
            UserInfo currentUser = db.UserInfos.Single(emp => emp.UserId == userID);
            Cart cart = currentUser.Cart;
            
            if (cart == null)
            {
                RedirectToAction("Error", "Cart");
            }
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
            return View(cart);
        }

        public ActionResult CheckOut()
        {

            var userID = User.Identity.GetUserId();
            UserInfo currentUser = db.UserInfos.Single(emp => emp.UserId == userID);
            Cart cart = currentUser.Cart;
            if (cart.PicturesInCart == null && cart.AlbumsInCart == null)
            {
                ViewBag.Message = "No items in cart";
                return RedirectToAction("Error", "Cart");
            }
            decimal total = CartHelper.getTotalFromCart(cart);
            ViewBag.hasEnough = true;
            if (currentUser.AccountBalance < total)
            {
                ViewBag.hasEnough = false;
            }
            ViewBag.CartTotal = total;

            return View(cart);
        }

        public ActionResult InsufficientFunds()
        {
            return View();
        }

        public ActionResult Submit()
        {
            var userID = User.Identity.GetUserId();
            UserInfo user = db.UserInfos.Single(emp => emp.UserId == userID);
            List<Transaction> picTrans = CartHelper.generatePictureTransactions(user);
            List<Transaction> albumTrans = CartHelper.generateAlbumTransactions(user);

            //Put album and picture transactions in one list
            List<Transaction> AllTransactions = CartHelper.mergeLists(picTrans, albumTrans);
            foreach (Transaction trans in albumTrans)
            {
                AllTransactions.Add(trans);
            }
            string email_msg = "Your Recent Order\n";
            //Update Account Balances and add Transactions to the db
            foreach (Transaction trans in AllTransactions)
            {
                email_msg += trans.ToString();
                decimal total = trans.TotalAmount;
                user.AccountBalance -= total;
                trans.Seller.AccountBalance += total;
                db.Transactions.Add(trans);
                db.SaveChanges();
                userHelp.SetLevel(trans.Seller);
            }

            email_msg += "Your current balance: " + user.AccountBalance + "\n";

            //Send email
            var message = new SendGridMessage();
            message.AddTo(user.User.Email);
            message.From = new MailAddress("NoReply@photobucket", "Photo Bucket");
            message.Subject = "Transaction Confirmation";
            message.Text = email_msg;

            var username = "azure_3637600ffcd6eff02a904908c19238f9@azure.com";
            var pswd = "DbsCchzC1i21kvr";

            var credentials = new NetworkCredential(username, pswd);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            transportWeb.DeliverAsync(message);

            user.Cart.PicturesInCart.Clear();
            user.Cart.AlbumsInCart.Clear();

            return View();


        }

        public ActionResult buyPicture(int picId)
        {
            UserInfo user = db.UserInfos.Include(c => c.User).FirstOrDefault();
            Picture pic = db.Pictures.Include(c => picId).FirstOrDefault();
            if (pic != null && user.OwnedPictures.Contains(pic))
            {
                ViewBag.Message = "You already own that picture";
                return RedirectToAction("Error", "Cart");
            }
            Cart cart = user.Cart;

            CartHelper helper = new CartHelper(db);
            if (helper.buyPicture(picId, cart.UserId))
            {
                ViewBag.Message = "Just added " + picId;
                return RedirectToAction("Index", "Cart");
            } else {
                return RedirectToAction("Error", "Cart");
            }

        }
        
        public ActionResult buyAlbum(int albumId)
        {
            UserInfo user = db.UserInfos.Include(c => c.User).FirstOrDefault();
            Cart cart = user.Cart;

            CartHelper helper = new CartHelper(db);
            if (helper.buyAlbum(albumId, cart.UserId)) {
                ViewBag.Message = "Just added " + albumId;
                return RedirectToAction("Index", "Cart");
            } else {
                return RedirectToAction("Error", "Cart");
            }
        }

        public ActionResult Error()
        {
            return View();
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
        public ActionResult Delete(int picId)
        {

            var userID = User.Identity.GetUserId();
            UserInfo currentUser = db.UserInfos.Single(emp => emp.UserId == userID);
            Cart cart = currentUser.Cart;
            if (cart == null)
            {
                return HttpNotFound();
            }

            foreach (Picture pic in cart.PicturesInCart)
            {
                if (pic.Id == picId)
                {
                    cart.PicturesInCart.Remove(pic);
                    db.SaveChanges();
                    break;
                }
            }
                            
            return RedirectToAction("Index", "Cart");
        }

    }
}
