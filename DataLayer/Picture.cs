using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DataLayer
{
    class Picture
    {


        private List<Tag> tagList = new List<Tag>();
        private List<UserInfo> likedList = new List<UserInfo>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        public uint NumberOfLikes { get; set; }

        public bool HasBeenReported { get; set; }

        [Required]
        public String OriginalImg { get; set; }  //The image as an url

        [Required]
        public String CompressedImg { get; set; }


        [Required]
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public virtual UserInfo Owner { get; set; }

        [ForeignKey("Album")]
        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }

        [InverseProperty("Pictures")]
        public virtual ICollection<Tag> Tags { get { return tagList; } }

        public virtual ICollection<UserInfo> LikedBy { get { return likedList; } }

    }
}
