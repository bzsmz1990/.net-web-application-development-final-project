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
using System.Net;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PhotoProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private UserInfoHelper userHelp = new UserInfoHelper(AlbumDetailsController.db);

        // GET: Cart
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            UserInfo currentUser = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
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
            UserInfo currentUser = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
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

        public async Task<ActionResult> Submit()
        {
            var userID = User.Identity.GetUserId();
            UserInfo user = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
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
                AlbumDetailsController.db.Transactions.Add(trans);
                AlbumDetailsController.db.SaveChanges();
                userHelp.SetLevel(trans.Seller);
            }
            user.Cart.PicturesInCart.Clear();
            user.Cart.AlbumsInCart.Clear();

            email_msg += "Your current balance: " + user.AccountBalance + "\n";

            //Send email
            await sendEmail(email_msg, user.User.Email);

            return View();


        }

        private async Task sendEmail(string email_msg, string email)
        {
            SendGridMessage message = new SendGridMessage();
            message.AddTo(email);
            message.From = new MailAddress("NoReply@photobucket", "Photo Bucket");
            message.Subject = "Transaction Confirmation";
            message.Text = email_msg;

            var username = "azure_3637600ffcd6eff02a904908c19238f9@azure.com";
            var pswd = "DbsCchzC1i21kvr";

            var credentials = new NetworkCredential(username, pswd);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(message);
            }
            else
            {
                Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }

        [Authorize]
        public ActionResult buyPicture(int picId)
        {
            var userID = User.Identity.GetUserId();
            UserInfo user = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
            Picture pic = AlbumDetailsController.db.Pictures.Single(c => c.Id == picId);
            if (pic != null && user.OwnedPictures.Contains(pic))
            {
                ViewBag.Message = "You already own that picture";
                return RedirectToAction("Error", "Cart");
            }
            Cart cart = user.Cart;
            cart.PicturesInCart = (cart.PicturesInCart ?? new List<Picture>());
            cart.PicturesInCart.Add(pic);
            AlbumDetailsController.db.SaveChanges();
            ViewBag.Message = "Just added " + picId;
            return RedirectToAction("Index", "Cart");

        }
        
        public ActionResult buyAlbum(int albumId)
        {
            var userID = User.Identity.GetUserId();
            UserInfo user = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
            Cart cart = user.Cart;


            CartHelper helper = new CartHelper(AlbumDetailsController.db);
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
            Cart cart = AlbumDetailsController.db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(AlbumDetailsController.db.UserInfos, "UserId", "FirstName");
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
                AlbumDetailsController.db.Carts.Add(cart);
                AlbumDetailsController.db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(AlbumDetailsController.db.UserInfos, "UserId", "FirstName", cart.UserId);
            return View(cart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = AlbumDetailsController.db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(AlbumDetailsController.db.UserInfos, "UserId", "FirstName", cart.UserId);
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
                AlbumDetailsController.db.Entry(cart).State = EntityState.Modified;
                AlbumDetailsController.db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(AlbumDetailsController.db.UserInfos, "UserId", "FirstName", cart.UserId);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public ActionResult DeletePic(int picId)
        {

            var userID = User.Identity.GetUserId();
            UserInfo currentUser = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
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
                    AlbumDetailsController.db.SaveChanges();
                    break;
                }
            }
                            
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult DeleteAl(int albumID)
        {
            var userID = User.Identity.GetUserId();
            UserInfo currentUser = AlbumDetailsController.db.UserInfos.Single(emp => emp.UserId == userID);
            Cart cart = currentUser.Cart;
            if (cart == null)
            {
                return HttpNotFound();
            }
            foreach (Album al in cart.AlbumsInCart)
            {
                if (al.Id == albumID)
                {
                    cart.AlbumsInCart.Remove(al);
                    AlbumDetailsController.db.SaveChanges();
                    break;
                }
            }

            return RedirectToAction("Index", "Cart");
        }

    }
}
