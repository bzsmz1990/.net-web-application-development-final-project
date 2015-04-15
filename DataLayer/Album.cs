using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class Album
    {

        private List<Picture> PicturesList = new List<Picture>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual UserInfo user { get; set; }

        public virtual ICollection<Picture> Pictures { get { return PicturesList; } }

        public double Cost { get; set; }


    }
}
