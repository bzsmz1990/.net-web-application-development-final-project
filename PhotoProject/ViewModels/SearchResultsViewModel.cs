using DataLayer;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoProject.ViewModels
{
    public class SearchResultsViewModel
    {
        public IPagedList<Picture> Pictures { get; set; }

        public IPagedList<Album> Albums { get; set; }
    }
}