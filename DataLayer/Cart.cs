using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class Cart
    {

        private List<Picture> PicturesList = new List<Picture>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual UserInfo UserInfo { get; set; }

        public virtual ICollection<Picture> PicturesInCart { get { return PicturesList; } }

        // Agatha: We could add this as optional so that you have the ability
        // to save a picture to purchase later and that shows up on the bottom 
        // of the cart page (similar to Amazon)
        public virtual ICollection<Picture> PicturesSavedForLater { get; set; }

    }
}
