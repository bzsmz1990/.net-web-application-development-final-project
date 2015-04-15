using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class UserInfo
    {


        private List<Picture> OwnedPicList = new List<Picture>();
        private List<Picture> LikedPicList = new List<Picture>();
        private List<UserInfo> FollowersList = new List<UserInfo>();
        private List<UserInfo> FollowingList = new List<UserInfo>();
        private List<Transaction> TransactionList = new List<Transaction>();
        private List<Transaction> SaleTransactionList = new List<Transaction>();
        private List<Album> AlbumList = new List<Album>();
        private Cart UserCart = new Cart();

        [Key]
        [Column(Order = 1)]
        public string Id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string FirstName { get; set; }

        [StringLength(1, ErrorMessage = "The {0} should be {1} character long")]
        public string MiddleInitial { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [DefaultValue(0)]
        public double AccountBalance { get; set; }

        [DefaultValue(false)]
        public Boolean isDeleted { get; set; }

        public virtual ICollection<Picture> OwnedPictures { get { return OwnedPicList; } }

        public virtual ICollection<Picture> LikedPictures { get { return LikedPicList; } }

        public virtual ICollection<UserInfo> Followers { get { return FollowersList; } }

        public virtual ICollection<UserInfo> Following { get { return FollowingList; } }

        public virtual ICollection<Transaction> PurchaseTransactions { get { return TransactionList; } }

        public virtual ICollection<Transaction> SaleTransactions { get { return SaleTransactionList; } }

        public virtual ICollection<Album> Albums { get { return AlbumList; } }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart Cart { get { return UserCart; } }
    }
}
