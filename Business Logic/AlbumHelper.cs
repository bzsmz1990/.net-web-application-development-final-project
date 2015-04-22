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

    }
}
