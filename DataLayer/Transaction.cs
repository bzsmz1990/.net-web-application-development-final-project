using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Transaction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Buyer")]
        [Required]
        public string BuyerId { get; set; }
        public virtual UserInfo Buyer { get; set; }

        //[ForeignKey("Seller")]
        [Required]
        public string SellerId { get; set; }
        public virtual UserInfo Seller { get; set; }

        [InverseProperty("SaleTransactions")]
        public virtual ICollection<Picture> PicturesBeingSold { get; set; }

        [InverseProperty("SaleTransactions")]
        public virtual ICollection<Album> AlbumsBeingSold { get; set; }

        public decimal TotalAmount { get; set; }
        
    }
}
