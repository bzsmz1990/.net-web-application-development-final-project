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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual UserInfo Buyer { get; set; }

        public virtual UserInfo Seller { get; set; }

        public virtual ICollection<Picture> PicturesBeingSold { get { return SoldPicturesList; } }

        //Added this in case we want to add the ability for the user to change a picture's price.
        public double CurrentPrice { get; set; }

    }
}
