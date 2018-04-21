using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdventureDocs.Models;
using System.Configuration;

namespace AdventureDocs.Helpers
{
    public static class SearchHelper
    {
        public static List<string> GetSuggestions(string query)
        {
            List<string> suggestions = new List<string>();

            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string searchServiceKey = ConfigurationManager.AppSettings["SearchServiceKey"];

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceKey));
            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient("customerindex");
            DocumentSearchResult<Customer> response = indexClient.Documents.Search<Customer>($"{query.Trim()}*");

            suggestions = (from result in response.Results
                           select result.Document.CompanyName).ToList();

            return suggestions;
        }
    }
}