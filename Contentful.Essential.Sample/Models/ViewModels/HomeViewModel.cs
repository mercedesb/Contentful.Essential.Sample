﻿using System.Collections.Generic;
using System.Linq;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            SearchModel = new PatternSearchModel();
        }
        public IEnumerable<Pattern> Patterns { get; set; }

        public PatternSearchModel SearchModel { get; set; }

        public IEnumerable<string> GalleryImages { get; set; }

        protected IEnumerable<IEnumerable<string>> _galleryRows;
        public IEnumerable<IEnumerable<string>> GalleryRows
        {
            get
            {
                if (_galleryRows == null)
                {
                    int i = 0;
                    _galleryRows = GalleryImages.GroupBy(x => i++ / 3);
                }
                return _galleryRows;
            }
        }
    }
}