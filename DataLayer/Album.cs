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
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public virtual UserInfo User { get; set; }

        [Required]
        public virtual ICollection<Picture> Pictures { get { return PicturesList; } }

        [Range(0, 100)]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }


    }
}
