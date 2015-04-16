﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UserInfo
    {

        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string FirstName { get; set; }

        [StringLength(1, ErrorMessage = "The {0} should be {1} character long")]
        public string MiddleInitial { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [DefaultValue(0)]
        public decimal AccountBalance { get; set; }

        [DefaultValue(false)]
        public Boolean isDeleted { get; set; }

        [DefaultValue(1), Required]
        public int Level { get; set; }

        public virtual ICollection<Picture> OwnedPictures { get; set; }

        public virtual ICollection<Picture> LikedPictures { get; set; }

        public virtual ICollection<UserInfo> Followers { get; set; }

        public virtual ICollection<UserInfo> Following { get; set; }

        public virtual ICollection<Transaction> PurchaseTransactions { get; set; }

        public virtual ICollection<Transaction> SaleTransactions { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
