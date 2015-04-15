using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class Transaction
    {

        private List<Picture> SoldPicturesList = new List<Picture>();
        private List<Album> SoldAlbumsList = new List<Album>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Buyer")]
        [Required]
        public string BuyerId { get; set; }
        public virtual UserInfo Buyer { get; set; }

        [ForeignKey("Seller")]
        [Required]
        public string SellerId { get; set; }
        public virtual UserInfo Seller { get; set; }

        public virtual ICollection<Picture> PicturesBeingSold { get { return SoldPicturesList; } }

        public virtual ICollection<Album> AlbumsBeingSold { get { return SoldAlbumsList; } }

        //Added this in case we want to add the ability for the user to change a picture's price.
        public double TotalPrice { get; set; }

    }
}
