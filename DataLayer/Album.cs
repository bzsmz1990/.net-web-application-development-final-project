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
    public class Album
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual UserInfo User { get; set; }

        [Display(Name = "Album Name")]
        [Column("AlbumName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<Picture> Pictures { get; set; }

        [Range(0, 100)]
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "AlbumPrice")]
        [Column("AlbumPrice")]
        public decimal Cost { get; set; }

        [DefaultValue(false)]
        public bool HasBeenReported { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "UploadTime")]
        public DateTime UploadTime { get; set; }

        public virtual ICollection<Transaction> SaleTransactions  { get; set; }

        public virtual ICollection<Cart> CurrentCarts { get; set; }

        public virtual ICollection<Cart> SavedForLaterCarts { get; set; }

    }
}
