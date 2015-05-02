using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class AlbumHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static AlbumHelper albumHelp = new AlbumHelper(db);

        public AlbumHelper(ApplicationDbContext context)
        {
            db = context;
        }

        public ICollection<Album> GetAllAlbums()
        {
            return db.Albums.ToList();
        }

        public ICollection<Album> GetAlbumsOrderedByMostPurchased()
        {
            return db.Albums.OrderByDescending(p => p.SaleTransactions.Count).ToList();
        }

        public ICollection<Album> GetAlbumsOrderedByMostRecent()
        {
            return db.Albums.OrderByDescending(p => p.UploadTime).ToList();
        }

        public List<Album> GetAlbumsWhereTitleHasWord(string searchString)
        {
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString))
            {
                return db.Albums.ToList();
            }

            return db.Albums.Where(p => p.Name.Contains(searchString)).ToList();
        }

        public Album createAlbumWithPicturesAndCost(List<Picture> pics, decimal cost, DateTime date) {
            if (pics.Count == 0)
            {
                return null;
            }
            Album album = new Album {Pictures = new List<Picture>(), Cost = cost, UploadTime = date };
            foreach (Picture p in pics)
            {
                album.Pictures.Add(p);
            }
            return album;
        }

        public Album createAlbumWithOnePicture(Picture pic)
        {
            if (pic == null) {
                return null;
            }
            Album album = new Album {Pictures = new List<Picture>(){pic}};
            return album;
        }

        public Album addPictureToAlbum(Picture pic, Album al)
        {
            if (pic == null || al == null)
            {
                return null;
            }
            al.Pictures = (al.Pictures ?? new List<Picture>());
            al.Pictures.Add(pic);
            return al;
        }

        public Album addCostToAlbum(decimal cost, Album al)
        {
            if (cost < 0 || cost > Decimal.MaxValue || al == null)
            {
                return null;
            }
            al.Cost = cost;
            return al;
        }

        public Album addNameToAlbum(string name, Album al)
        {
            if (name == null || al == null)
            {
                return null;
            }
            al.Name = name;
            return al;
        }
    
        public ICollection<Album> FilterListByPictureType(string searchTerm, DataLayer.Picture.ValidFileType PictureType)
        {
            if (searchTerm == null)
            {
                return null;
            }

            List<Album> pictures = GetAlbumsWhereTitleHasWord(searchTerm);

            pictures.RemoveAll(a => a.Pictures != null && a.Pictures.Any(p => p.PictureType == PictureType));

            return pictures;
        }

    }
}
