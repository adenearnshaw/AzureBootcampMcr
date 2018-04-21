using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;
using AdventureDocs.Models;
using System.Configuration;

namespace AdventureDocs.Helpers
{
    public class DocumentHelper
    {
        public static DocumentClient GetDocumentClient()
        {
            string endpointUrl = ConfigurationManager.AppSettings["DocumentDBEndpointUrl"];
            string primaryKey = ConfigurationManager.AppSettings["DocumentDBKey"];
            DocumentClient client = new DocumentClient(new Uri(endpointUrl), primaryKey);
            return client;
        }

        public static async Task<List<string>> GetAvailableCollectionNamesAsync(DocumentClient client)
        {
            List<string> collections = new List<string>();

            try
            {
                var dbFeed = await client.ReadDatabaseFeedAsync();
                var defaultDb = dbFeed.FirstOrDefault();

                if (defaultDb != null)
                {
                    FeedResponse<DocumentCollection> collFeed = await client.ReadDocumentCollectionFeedAsync(defaultDb.SelfLink);

                    collections = (from feed in collFeed select feed.Id).ToList();
                }
            }
            catch (Exception)
            {
            }

            return collections;
        }

        public static List<SearchResultInformation> GetOrdersByOrder(DocumentClient client, string countryName)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<OrderInformation> orderQuery = client.CreateDocumentQuery<OrderInformation>(
                    UriFactory.CreateDocumentCollectionUri("CustomerOrders", "Orders"), queryOptions)
                    .Where(f => f.Customer.Orders.ShipCountry.ToLower().StartsWith(countryName.ToLower()));

            var orderItems = orderQuery.ToList();

            var results = (from item in orderItems
                           select new SearchResultInformation()
                           {
                               Title = item.Customer.CompanyName,
                               Description = item.Customer.Orders.ShipCountry,
                               DocumentContent = JsonConvert.SerializeObject(item),

                           }).ToList();


            return results.Select(r => r.Title).Distinct().Select(title => results.First(r => r.Title == title)).ToList();
        }

        public static List<SearchResultInformation> GetOrdersByCustomer(DocumentClient client, string companyName)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<OrderInformation> orderQuery = client.CreateDocumentQuery<OrderInformation>(
                    UriFactory.CreateDocumentCollectionUri("CustomerOrders", "Orders"), queryOptions)
                     .Where(f => f.Customer.CompanyName.ToLower().StartsWith(companyName.ToLower()));


            var orderItems = orderQuery.ToList();

            List<SearchResultInformation> results = (from item in orderItems
                                                     select new SearchResultInformation()
                                                     {
                                                         Title = item.Customer.CompanyName,
                                                         Description = item.Customer.Country,
                                                         DocumentContent = JsonConvert.SerializeObject(item),

                                                     }).ToList();

            return results.Select(r => r.Title).Distinct().Select(title => results.First(r => r.Title == title)).ToList();
        }

        public static List<SearchResultInformation> GetOrdersByProduct(DocumentClient client, string productName)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<OrderInformation> orderQuery = client.CreateDocumentQuery<OrderInformation>(
                    UriFactory.CreateDocumentCollectionUri("CustomerOrders", "Orders"), queryOptions)
                    .Where(f => f.Customer.Orders.Details.Product.ProductName.ToLower().StartsWith(productName.ToLower()));

            var orderItems = orderQuery.ToList();

            var results = (from item in orderItems
                           select new SearchResultInformation()
                           {
                               Title = item.Customer.Orders.Details.Product.ProductName,
                               Description = item.Customer.Orders.Details.Product.QuantityPerUnit,
                               DocumentContent = JsonConvert.SerializeObject(item),

                           }).ToList();


            return results.Select(r => r.Title).Distinct().Select(title => results.First(r => r.Title == title)).ToList();
        }
    }
}