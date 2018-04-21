using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AdventureDocs.Models;

namespace AdventureDocs.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new OrderViewModel() { SearchResults = new List<SearchResultInformation>() };
            var documentClient = Helpers.DocumentHelper.GetDocumentClient();
            var availableCollections = await Helpers.DocumentHelper.GetAvailableCollectionNamesAsync(documentClient);
            var searchResults = (List<SearchResultInformation>)TempData["SearchResults"];
            var searchQuery = (string)Request["SearchQuery"];

            if (searchResults != null)
            {
                model.SearchQuery = (string)TempData["SearchQuery"];
                model.SearchResults = (List<SearchResultInformation>)TempData["SearchResults"];
                model.SelectedCollectionName = (string)TempData["SelectedCollectionName"];
                model.SearchResultTitle = $"{model.SelectedCollectionName}";
                model.SearchResultDescription = $"The following results were found in {model.SelectedCollectionName} for '{model.SearchQuery.ToUpper()}':";
            }
            else if (!string.IsNullOrEmpty(searchQuery))
            {
                model.SearchQuery = searchQuery;
                searchResults = Helpers.DocumentHelper.GetOrdersByCustomer(documentClient, searchQuery);
                model.SearchResults = searchResults;
                model.SelectedCollectionName = "Customers";
                model.SearchResultTitle = $"{model.SelectedCollectionName}";
                model.SearchResultDescription = $"The following results were found in {model.SelectedCollectionName} for '{model.SearchQuery.ToUpper()}':";
            }
            else
            {
                model.SearchQuery = "";
                model.SelectedCollectionName = "Customers";
                model.SearchResultTitle = "";
                model.SearchResultDescription = "";
            }

            model.Collections = availableCollections;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Lookup()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpGet]
        public ActionResult ViewSource(string[] content)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = content[0]
            };
        }

        [HttpPost]
        public ActionResult Search(OrderViewModel model)
        {
            ViewBag.Message = "Your application description page.";
            string searchQuery = model.SearchQuery + "";
            var documentClient = Helpers.DocumentHelper.GetDocumentClient();
            List<SearchResultInformation> searchResults = new List<SearchResultInformation>();

            switch (model.SelectedCollectionName)
            {
                case "Customers":
                    searchResults = Helpers.DocumentHelper.GetOrdersByCustomer(documentClient, searchQuery);
                    break;
                case "Products":
                    searchResults = Helpers.DocumentHelper.GetOrdersByProduct(documentClient, searchQuery);
                    break;
                case "Orders":
                    searchResults = Helpers.DocumentHelper.GetOrdersByOrder(documentClient, searchQuery);
                    break;
                default:
                    break;
            }

            TempData["SearchQuery"] = searchQuery;
            TempData["SearchResults"] = searchResults;
            TempData["SelectedCollectionName"] = model.SelectedCollectionName;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AutoSearch(string item)
        {
            ViewBag.Message = "Your application description page.";

            string searchQuery = item + "";

            TempData["SearchQuery"] = searchQuery;
            TempData["SelectedCollectionName"] = "Customers";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Suggest(string term)
        {
            List<string> suggestions = new List<string>();

            suggestions = Helpers.SearchHelper.GetSuggestions(term);

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = suggestions
            };
        }
    }
}