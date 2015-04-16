using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Album
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public virtual UserInfo User { get; set; }

        [Required]
        public virtual ICollection<Picture> Pictures { get; set; }

        [Range(0, 100)]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        public virtual ICollection<Transaction> SaleTransactions  { get; set; }

        public virtual ICollection<Cart> CurrentCarts { get; set; }

        public virtual ICollection<Cart> SavedForLaterCarts { get; set; }

    }
}
