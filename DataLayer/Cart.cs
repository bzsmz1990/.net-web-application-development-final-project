using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Cart
    {

        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public virtual UserInfo User { get; set; }

        [InverseProperty("CurrentCarts")]
        public virtual ICollection<Picture> PicturesInCart { get; set; }

        [InverseProperty("CurrentCarts")]
        public virtual ICollection<Album> AlbumsInCart { get; set; }

        [InverseProperty("SavedForLaterCarts")]
        public virtual ICollection<Picture> PicturesSavedForLater { get; set; }

        [InverseProperty("SavedForLaterCarts")]
        public virtual ICollection<Album> AlbumsSavedForLater { get; set; }

    }
}
