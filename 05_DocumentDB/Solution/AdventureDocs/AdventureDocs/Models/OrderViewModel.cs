using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureDocs.Models
{
    public class OrderViewModel
    {
        public string SearchQuery { get; set; }
        public List<SearchResultInformation> SearchResults { get; set; }
        public List<string> Collections { get; set; }
        public string SelectedCollectionName { get; set; }
        public string SearchResultTitle { get; set; }
        public string SearchResultDescription { get; set; }
    }
}