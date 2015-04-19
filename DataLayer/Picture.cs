using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace DataLayer
{
    public class Picture
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Owner")]
        [Required] 
        public string OwnerId { get; set; }
        public virtual UserInfo Owner { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, 100, ErrorMessage = "The {0} cannot be less than 0")]
        public decimal Cost { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "UploadTime")]
        public DateTime UploadTime { get; set; }

        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "The number of {0} cannot be less than 0.")]
        public int NumberOfLikes { get; set; }

        [DefaultValue(false)]
        public bool HasBeenReported { get; set; }

        [Required]
        public byte[] OriginalImg { get; set; }  //The image as an url

        [Required]
        public byte[] CompressImg { get; set; }

        [Required]
        public string PictureType { get; set; }

        public bool Hidden { get; set; }

        [ForeignKey("Album")]
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        [InverseProperty("Pictures")]
        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<UserInfo> LikedBy { get; set; }

        public virtual ICollection<Cart> CurrentCarts { get; set; }

        public virtual ICollection<Cart> SavedForLaterCarts { get; set; }

        public virtual ICollection<Transaction> SaleTransactions { get; set; }

    }
}
