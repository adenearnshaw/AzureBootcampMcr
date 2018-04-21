<a name="HOLTitle"></a>
# Using Azure Cosmos DB and the DocumentDB API #

---

<a name="Overview"></a>
## Overview ##

Whether it targets businesses, consumers, or both, an app is only as meaningful as the data that drives it. With consumer and organizational requirements changing constantly, as well as the need to store, index, and optimize data and structures as they change, the need for a more open, flexible, and schema-agnostic data solution has been become essential. Azure Cosmos DB addresses this need and more and makes it easy to adjust and adapt data models on the fly, as business logic and application scenarios change.

[Azure Cosmos DB](https://azure.microsoft.com/services/cosmos-db/) is Microsoft's globally distributed, multi-model database service for mission-critical applications. It supports turn-key global distribution, elastic scaling of throughput and storage worldwide, single-digit millisecond latencies at the 99th percentile, five well-defined consistency levels, and guaranteed high availability, all backed by industry-leading SLAs. Azure Cosmos DB automatically indexes data without requiring you to deal with schema and index management. It is multi-model and supports document, key-value, graph, and columnar data models.

[DocumentDB](https://docs.microsoft.com/azure/cosmos-db/documentdb-introduction) is a feature of Cosmos DB implementing a fully managed NoSQL database service built for fast performance, high availability, elastic scaling, and ease of development. As a schema-free NoSQL database, DocumentDB provides rich and familiar SQL query capabilities over JSON data, ensuring that 99% of your reads are served under 10 milliseconds and 99% of your writes are served under 15 milliseconds. These unique benefits make DocumentDB a great fit for Web, mobile, gaming, IoT, and many other applications that need seamless scale and global replication.

In this lab, you will deploy a DocumentDB database under Cosmos DB to store customer and product order information for the fictitious company *Adventure Works*, and you will connect it to Azure Search to index the data and implement auto-suggest. You will also write a Web app that uses the database and demonstrates how easily applications can consume data from DocumentDB.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create an Azure Cosmos DB account
- Create DocumentDB collections and populate them with documents
- Create an Azure Search service and and use it to index DocumentDB data
- Access DocumentDB collections from your apps
- Query the Azure Search service connected to a DocumentDB database

<a name="Prerequisites"></a>
### Prerequisites ###

The following are required to complete this hands-on lab:

- An active Microsoft Azure subscription. If you don't have one, [sign up for a free trial](https://azure.microsoft.com/en-us/free/).
- [Visual Studio 2017 Community edition](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx) or higher with the "ASP.NET and web development" and "Azure development" workloads installed
 
---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Exercise 1: Create a Cosmos DB account](#Exercise1)
- [Exercise 2: Create a DocumentDB database and collections](#Exercise2)
- [Exercise 3: Populate collections with documents](#Exercise3)
- [Exercise 4: Connect Azure Search](#Exercise4)
- [Exercise 5: Build a Web app](#Exercise5)
- [Exercise 6: Add auto-suggest](#Exercise6)
 
Estimated time to complete this lab: **60** minutes.

<a name="Exercise1"></a>
## Exercise 1: Create a Cosmos DB account ##

The first step in working with DocumentDB is to create a Cosmos DB account to hold databases, collections, and documents. In this exercise, you will create a Cosmos DB account using the Azure Portal.

1.	Open the [Azure Portal](https://portal.azure.com) in your browser. If you are asked to sign in, do so with your Microsoft Account.

1.	Click **+ New**, followed by **Databases** and **Azure Cosmos DB**.	

    ![Creating a Cosmos DB account](Images/portal-create-new-documentdb.png)

    _Creating a Cosmos DB account_

1. In the "Azure Cosmos DB" blade, give the account a unique name such as "documentdbhol" and make sure a green check mark appears next to it. (You can only use numbers and lowercase letters since the name becomes part of a DNS name.) Select **SQL (DocumentDB)** as the **API**. Then select **Create new** under **Resource group** and name the resource group "DocumentDBResourceGroup." Select the **Location** nearest you, and then click the **Create** button.

    ![Specifying Cosmos DB parameters](Images/portal-configure-new-documentdb.png)

    _Specifying Cosmos DB parameters_

1. Click **Resource groups** in the ribbon on the left side of the portal, and then click the resource group created for the Cosmos DB account.
 
    ![Opening the resource group](Images/open-resource-group.png)

    _Opening the resource group_

1. Wait until "Deploying" changes to "Succeeded," indicating that the Cosmos DB account has been deployed. You can click the **Refresh** button at the top of the blade to refresh the deployment status.

    ![Viewing the deployment status](Images/deployment-status.png)

    _Viewing the deployment status_

Your Cosmos DB account is now provisioned and ready for you to start working with it.

<a name="Exercise2"></a>
## Exercise 2: Create a DocumentDB database and collections ##

Now that you’ve created a Cosmos DB account, the next step is to create a database and add collections to it in preparation for storing documents. In this exercise, you will create a database and add three collections to it for storing information about customers, products, and orders.

1. Click the Cosmos DB account that you deployed in [Exercise 1](#Exercise1).

    ![Opening the Cosmos DB account](Images/open-documentdb.png)

    _Opening the Cosmos DB account_

1. Click **+ Add Collection**.

    ![Adding a collection](Images/add-collection.png)

    _Adding a collection_

1. Enter "Customers" (without quotation marks) as the **Collection Id** and make sure **STORAGE CAPACITY** is set to **Fixed (10 GB)**. Select **Create New** under **DATABASE** and specify "CustomerOrders" as the database name. Then click the **OK** button.

    ![Creating a Customers collection](Images/create-collection-and-database.png)

    _Creating a Customers collection_

1. Click **+ Add Collection** again. Fill in the form as shown below to create a second collection named "Orders." Be sure to add it to the existing database ("CustomerOrders") rather than create a new database. Then click **OK**.

    ![Adding an Orders collection](Images/add-orders-collection.png)

    _Adding an Orders collection_

1. Click **+ Add Collection** again. Create a third collection named "Products" with the same settings as the "Customers" and "Orders" collections. Once more, be sure to add it to the existing database ("CustomerOrders") rather than create a new database.

The next step is to upload documents containing data regarding customers, products, and orders to the collections you created.

<a name="Exercise3"></a>
## Exercise 3: Populate collections with documents ##

There are several ways to populate DocumentDB collections with documents, including programmatic import via the [Azure SDK](https://www.nuget.org/packages/Microsoft.Azure.DocumentDB/) and the [Microsoft DocumentDB Data Migration Tool](https://www.microsoft.com/en-us/download/details.aspx?id=46436). In this exercise, you will populate your collections with data by uploading JSON documents through the Azure Portal.

1. Click **Document Explorer** in the menu on the left. Make sure **Customers** is selected in the drop-down list of collections, and click **Upload**.

    ![Opening Document Explorer](Images/open-document-explorer.png)

    _Opening Document Explorer_

1. Click the folder icon. Select all of the files in this lab's "Resources/Customers" folder, and then click the **Upload** button.

    ![Uploading customer data](Images/upload-customer-data.png)

    _Uploading customer data_

	Each of the files you uploaded is a JSON document containing information about one customer. Here is one of those files:

	```JSON
	{
	  "CustomerID": "ALFKI",
	  "CompanyName": "Alfreds Futterkiste",
	  "ContactName": "Maria Anders",
	  "ContactTitle": "Sales Representative",
	  "Address": "Obere Str. 57",
	  "City": "Berlin",
	  "Region": null,
	  "PostalCode": "12209",
	  "Latitude": 52.54971,
	  "Longitude": 13.49041,
	  "Country": "Germany",
	  "Phone": "030-0074321",
	  "Fax": "030-0076545"
	}
	```

	There are 91 files in all, so the Customers collection is now populated with data for 91 customers.

1. Close the "Upload Document" blade and return to the "Document Explorer" blade. Select **Orders** from the drop-down list of collections and click **Upload**. Then upload all of the files in this lab's "Resources/Orders" folder to the Orders collection.

1. Repeat this process to upload all of the files in this lab's "Resources/Products" folder to the Products collection.

1. The next step is to validate the document uploads by querying one or more of the collections. Click **Query Explorer** in the menu on the left. Select **Orders** in the drop-down list of collections, and then click the **Run Query** button.

	> Although you won't use DocumentDB's rich query capabilities directly in this lab, be aware that DocumentDB supports a variation of the SQL query language for extracting information from JSON data. For more information, and plenty of examples, see https://docs.microsoft.com/azure/documentdb/documentdb-sql-query.

    ![Querying the Orders collection](Images/open-query-explorer.png)

    _Querying the Orders collection_

1. Confirm that you see the query results below.

    ![Query results](Images/query-results.png)

    _Query results_

1. If you would like to confirm that the product and customer uploads succeeded too, run the same query against the Products and Customers collections.

The database that you created now has three collections that are populated with data. Now let's index the data so searches can be performed quickly.

<a name="Exercise4"></a>
## Exercise 4: Connect Azure Search ##

One of the benefits of using DocumentDB is that it integrates easily with [Azure Search](https://azure.microsoft.com/services/search/). Azure Search is a managed Search-as-a-Service solution that delegates server and infrastructure management to Microsoft and lets you index data sources for lightning-fast searches. Search can be accessed through a simple [REST API](https://docs.microsoft.com/rest/api/searchservice/) or with the [Azure Search SDK](https://docs.microsoft.com/azure/search/search-howto-dotnet-sdk), enabling you to employ it in Web apps, mobile apps, and other types of applications. In this exercise, you will deploy an Azure Search service and connect it to the DocumentDB database that you created in [Exercise 2](#Exercise2).

1.	In the Azure Portal, click **+ New**, followed by **Web + Mobile** and then **Azure Search**.
	
    ![Creating a new Azure Search service](Images/portal-create-new-search-service.png)

    _Creating a new Azure Search service_

1. In the "New Search Service" blade, enter a unique name in the **URL** box and make sure a green check mark appears next to it. (You can only use numbers and lowercase letters since the name becomes part of a DNS name.) Place the service in the same resource group and location you chose for the Cosmos DB account in [Exercise 1](#Exercise1).

	Now click **Pricing tier** and select the **Free** tier. Finish up by clicking the **Select** button at the bottom of the "Choose your pricing tier" blade, followed by the **Create** button at the bottom of the "New Search Service" blade.

    ![Specifying Search parameters](Images/portal-configure-new-search-service.png)

    _Specifying Search parameters_

1. Click **Resource groups** in the ribbon on the left side of the portal, and then click the resource group containing the DocumentDB account and Search service.
 
    ![Opening the resource group](Images/open-resource-group.png)

    _Opening the resource group_

1. Wait until "Deploying" changes to "Succeeded," indicating that the Search service has been deployed. (You can click the **Refresh** button at the top of the blade to refresh the deployment status.) Then click the Search service.

    ![Opening the Search service](Images/open-search.png)

    _Opening the Search service_

1. Click **Import data**.
	
    ![Importing data](Images/portal-select-import-data.png)

    _Importing data_

1. Click **Data Source**, followed by **DocumentDB**. In the "New data source" blade, type "customers" into the **Name** field. Click **Select an account** and select the Cosmos DB account you created in [Exercise 1](#Exercise1). Select the **CustomerOrders** database and the **Customers** collection. Then click **OK**.

    ![Connecting to a data source](Images/portal-connect-search-datasource.png)

    _Connecting to a data source_

1. Click **Customize target index** in the "Import data" blade. In the "Index" blade, type "customerindex" (without quotation marks) into the **Index name** field, and check all five boxes in the CompanyName row. Then click **OK**.

    ![Configuring a search index](Images/portal-configure-search-index.png)

    _Configuring a search index_

1. Click **Import your data** in the "Import data" blade. In the "Create an Indexer" blade, type "customerindexer" into the **Name** field and click **OK**. Finish up by clicking the **OK** button at the bottom of the "Import data" blade.
	
    ![Configuring a search indexer](Images/portal-configure-search-indexer.png)

    _Configuring a search indexer_
 
With the DocumentDB database deployed and an Azure Search service connected to it, it is time to put both to work by building a Web app that uses them to display customer, product, and order information.

<a name="Exercise5"></a>
## Exercise 5: Build a Web app ##

In this exercise, you will build an ASP.NET MVC Web app with Visual Studio. The app will connect to the DocumentDB database you deployed and populated with data in previous exercises and provide a browser-based front-end for viewing and searching the data.

1.	Start Visual Studio and use the **File -> New -> Project** command to create a new Visual C# ASP.NET Web Application project named "AdventureDocs" (short for "Adventure Works Documents").
 
    ![Creating a new Web Application project](Images/vs-create-new-web-app.png)

    _Creating a new Web Application project_

1. In the ensuing dialog, make sure **MVC** is selected. Then click **OK**.
 
    ![Configuring the project](Images/vs-configure-web-app.png)

    _Configuring the project_

1. Use Visual Studio's **Debug -> Start Without Debugging** command (or simply press **Ctrl+F5**) to launch the application in your browser. Here's how the application looks in its present state:
	 
    ![The initial application](Images/vs-initial-application.png)

    _The initial application_

1. Close the browser and return to Visual Studio. In the Solution Explorer window, right-click the **AdventureDocs** project and select **Manage NuGet Packages...**. Click **Browse**. Then type "documentdb" (without quotation marks) into the search box. Click **Microsoft.Azure.DocumentDB** to select the Azure DocumentDB .NET Client Library from NuGet. Finally, click **Install** to install the latest stable version of the package. This package contains APIs for accessing Azure DocumentDB from .NET applications. OK any changes and accept any licenses presented to you.
	 
    ![Installing Microsoft.Azure.DocumentDB](Images/vs-add-documentdb-package.png)

    _Installing Microsoft.Azure.DocumentDB_

1. Repeat this process to add the NuGet package named **Microsoft.Azure.Search** to the project. This package contains APIs for accessing Azure Search from .NET applications. Once more, OK any changes and accept any licenses presented to you.	
	
    ![Installing Microsoft.Azure.Search](Images/vs-add-search-package.png)

    _Installing Microsoft.Azure.Search_

1. Repeat this process to add the NuGet package named **jQuery.UI.Combined** to the project. This package contains APIs for jQuery user-interface elements. Once more, OK any changes and accept any licenses presented to you.
	
    ![Installing jQuery.UI.Combined](Images/vs-add-jquery-package.png)

    _Installing jQuery.UI.Combined_

1. Open **Web.config** and add the following statements to the ```<appSettings>``` section:

	```XML
	<add key="DocumentDBEndpointUrl" value="DOCUMENTDB_ENDPOINT" />
	<add key="DocumentDBKey" value="DOCUMENTDB_KEY" />
	```

1. Return to the Azure Portal and open the blade for the Cosmos DB account that you created in [Exercise 1](#Exercise1). Click **Keys**. Then click **Read-only Keys**, and click the **Copy** button to the right of the **URI** box to copy the DocumentDB URI to the clipboard.
	
    ![Copying the DocumentDB URI](Images/copy-documentdb-uri.png)

    _Copying the DocumentDB URI_

1.	Return to Visual Studio. In **Web.config**, replace DOCUMENTDB_ENDPOINT with the URI on the clipboard.
	
1. Return to the Azure Portal and click the **Copy** button to the right of **PRIMARY READ-ONLY KEY** to copy the access key the clipboard.
	
1. Return to Visual Studio. In **Web.config**, replace DOCUMENTDB_KEY with the key on the clipboard.

1. Open **_Layout.cshtml** in the project's "Views/Shared" folder. Then replace the file's contents with the following statements: 

	```HTML
	<!DOCTYPE html>
	<html>
	<head>
	    <meta charset="utf-8" />
	    <meta name="viewport" content="width=device-width, initial-scale=1.0">
	    <title>@ViewBag.Title</title>
	    @Styles.Render("~/Content/css")
	    @Scripts.Render("~/bundles/modernizr")
	    @Styles.Render("~/Content/css")
	    @Styles.Render("~/Content/themes/base/css")
	    @Scripts.Render("~/bundles/modernizr")
	    @Scripts.Render("~/bundles/jquery")
	    @Scripts.Render("~/bundles/bootstrap")
	    @Scripts.Render("~/bundles/jqueryui")
	</head>
	<body>
	    <div class="navbar navbar-inverse navbar-fixed-top">
	        <div class="container">
	            <div class="navbar-header">
	                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
	                    <span class="icon-bar"></span>
	                    <span class="icon-bar"></span>
	                    <span class="icon-bar"></span>
	                </button>
	                @Html.ActionLink("AdventureDB", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
	            </div>
	            <div class="navbar-collapse collapse">
	                <ul class="nav navbar-nav">
	                    <li>@Html.ActionLink("Document Search", "Index", "Home")</li>
	                    <li>@Html.ActionLink("Customer Lookup", "Lookup", "Home")</li>
	                </ul>
	            </div>
	        </div>
	    </div>
	    <div class="container body-content">
	        @RenderBody()
	        <hr />
	        <footer>
	            <p class="text-muted">All rights reserved. Copyright &copy;@DateTime.Now.Year AdventureDB.</p>
	        </footer>
	    </div>
	
	    @Scripts.Render("~/bundles/bootstrap")
	    @RenderSection("scripts", required: false)
	</body>
	</html>
	```

1. Right-click the "Models" folder in Solution Explorer and use the **Add -> Class...** command to add a class file named **OrderInformation.cs**. Then replace the empty ```OrderInformation``` class with the following class definitions:

	```C#
 	[Serializable]
    public class OrderInformation
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public object Region { get; set; }
        public string PostalCode { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Orders Orders { get; set; }
    }

    public class Orders
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int ShipVia { get; set; }
        public float Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public object ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public Details Details { get; set; }
    }

    public class Details
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public Product Product { get; set; }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public int UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
	```

1. Right-click the "Models" folder again and use the **Add -> Class...** command to add a file named **SearchResultInformation.cs** to the folder. Replace the empty ```SearchResultInformation``` class with the following class definition:

	```C#
	public class SearchResultInformation
	{
	    public string Title { get; set; }
	    public string Description { get; set; }
	    public string DocumentContent { get; set; }
	}
	```

1. Repeat this process to add an ```OrderViewModel``` class to the "Models" folder, and replace the empty class with the following class definition:
	
	```C#
	public class OrderViewModel
	{
	    public string SearchQuery { get; set; }
	    public List<SearchResultInformation> SearchResults { get; set; }
	    public List<string> Collections { get; set; }
	    public string SelectedCollectionName { get; set; }
	    public string SearchResultTitle { get; set; }
	    public string SearchResultDescription { get; set; }
	}
	```
1.	In the Solution Explorer window, right-click the **AdventureDocs** project and use the **Add -> New Folder** command to add a folder named "Helpers" to the project.
	
1. Right-click the "Helpers" folder and use the **Add -> Class...** command to add a file named **DocumentHelper.cs** to the folder. Replace the contents of the file with the following statements:

	```C#
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
	```

1. Open **HomeController.cs** in the project's "Controllers" folder. Add the following ```using``` statements to the top of the file:

	```C#
	using System.Threading.Tasks;
	using AdventureDocs.Models;
	```

1. Replace the ```Index``` method in **HomeController.cs** with the following implementation:

	```C#
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
	
	```
	
1. Add the following methods to the ```HomeController``` class in **HomeController.cs**:
	
	```C#
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
	```

1. Open **Index.cshmtl** in the "Views/Home" folder and replace its contents with the following statements:

	```HTML
	@{
	    ViewBag.Title = "AdventureDocs";
	}
	
	<div class="row">
	    @model AdventureDocs.Models.OrderViewModel
	    <div>
	        <h2>Document Search</h2>
	        <p>
	            To search documents in your Azure DocumentDB database, enter a value, select a DocumentDB collection, and click Search.
	        </p>
	
	        @using (Html.BeginForm("Search", "Home", FormMethod.Post))
	        {
	            <div>Search for:</div>
	            @Html.TextBoxFor(o => Model.SearchQuery)
	            <p></p>
	            <div>Select a collection:</div>
	            @Html.DropDownListFor(x => x.SelectedCollectionName, new SelectList(Model.Collections))
	            <input type="submit" value="Search">
	        }
	
	        <div>
	            <h4>@Html.DisplayFor(o => Model.SearchResultTitle)</h4>
	            <div>@Html.DisplayFor(o => Model.SearchResultDescription)</div>
	            <table style="margin:10px" border="0" cellpadding="3">
	                @foreach (var item in Model.SearchResults)
	                {
	                    <tr>
	                        <td>
	                            <strong>@Html.DisplayFor(modelItem => item.Title)</strong>
	                        </td>
	                        <td>
	                            <em>@Html.DisplayFor(modelItem => item.Description)</em>
	                        </td>
	
	                        <td>
	                            @Html.ActionLink(
	                            linkText: "[view document]",
	                            actionName: "ViewSource",
	                            controllerName: "Home",
	                            routeValues: new { content = item.DocumentContent },
	                            htmlAttributes: null)
	                        </td>
	                    </tr>
	                }
	            </table>
	        </div>
	    </div>
	</div>
	```

1. Find **About.cshmtl** in the "Views/Home" folder. Right-click the file and use the **Rename** command to change its name to **Lookup.cshtml**. This is the view that will serve as the document lookup page.

1. Replace the contents of **Lookup.cshtml** with the following statements:

	```HTML
	@Scripts.Render("~/bundles/jqueryui")
	
	<script type="text/javascript">
	    $(document).ready(function () {
	 
	        $('#customers').autocomplete({
	            source: '@Url.Action("Suggest")',
	            autoFocus: true,
	            select: function (event, ui) {
	
	                if (ui.item) {
	                    $("#SearchQuery").val(ui.item.value);
	                    $("form").submit();
	                }
	            }
	        }); 
	    })
	</script>
	
	<div class="row">
	    <div class="col-md-4">
	        <h2>Customer Lookup</h2>
	        <p>
	            To search documents in your Azure DocumentDB database, enter a value and select an autosuggested customer.
	        </p>
	        <div>Search for:</div>
	        <input id="customers" name="customers">
	        <form action="/" method="post">
	            <input hidden="hidden" id="SearchQuery" name="SearchQuery" type="text" />
	        </form>
	    </div>
	    <div style="height:400px" class="col-md-4"></div>
	</div>
	```

1. Open **BundleConfig.cs** in the project's "App_Start" folder. Add the following code at the end of the ```RegisterBundles``` method:

	```C#
	bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));
	bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
	            "~/Content/themes/base/jquery.ui.core.css",
	            "~/Content/themes/base/jquery.ui.autocomplete.css",
	            "~/Content/themes/base/jquery.ui.theme.css"));
	```

1. Use Visual Studio's **Debug -> Start Without Debugging** command (or press **Ctrl+F5**) to launch the application in your browser. 

1. **Type** the letter "a" in the **Search for** box and click the **Search** button. Confirm that a list of customer names starting with A appears on the page:
	
    ![Searching for customer names that begin with A](Images/vs-documentsearch-01.png)

    _Searching for customer names that begin with A_

1. Select **Products** from the list of collections and click the **Search** button. Confirm that a list of product names starting with A appears on the page:
	
    ![Searching for product names that begin with A](Images/vs-documentsearch-02.png)

    _Searching for product names that begin with A_

1. Replace the letter "a" in the **Search for** box with the letter "m." Then select **Orders** and click the **Search** button. Confirm that a list of orders appears, with the countries they were shipped to listed on the right:
	
    ![Searching for orders shipped to countries that begin with M](Images/vs-documentsearch-03.png)

    _Searching for orders shipped to countries that begin with M_

1. The Orders listing displays the CompanyName and ShipRegion values from the orders returned in the search results. To view the entire order, click **[view document]** to the right of an order. The result is the JSON defining the order:
	
    ![Viewing the JSON for an order](Images/vs-documentsearch-04.png)

    _Viewing the JSON for an order_	

This is a great start, and it demonstrates how an ASP.NET MVC Web app can access data stored in the cloud in a DocumentDB database. But right now, the search UI is somewhat clumsy; you have to enter letters blindly, without any feedback to tell you whether there are any matching documents. Let's enhance the UI by adding auto-suggest.

<a name="Exercise6"></a>
## Exercise 6: Add auto-suggest ##

Azure Search enables super-fast retrieval of indexed values in the data stores it is connected to. In this exercise, you will leverage that speed to add an auto-suggest list to customer search to provide feedback to the user as he or she types.

1. Return to Visual Studio. Right-click the "Helpers" folder and use the **Add -> Class...** command to add a file named **SearchHelper.cs** to the folder. Replace the contents of the file with the following statements:

	```C#
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
	```

1. Open **Web.config** and add the following statements to the ```<appSettings>``` section, replacing SEARCH_SERVICE_NAME with the name you assigned to the Azure Search service in Exercise 4, Step 2:

	```XML
	<add key="SearchServiceName" value="SEARCH_SERVICE_NAME" />
	<add key="SearchServiceKey" value="SEARCH_SERVICE_KEY" />
	```

1. Return to the Azure Portal and open the blade for the Azure Search service. Click **Keys**, followed by **Manage query keys**. Then copy the query key to the clipboard.

	> The purpose of query keys is to allow applications to query the Search service and to do so securely.

    ![Copying the query key to the clipboard](Images/portal-copy-search-query-key.png)

    _Copying the query key to the clipboard_

1. Return to Visual Studio. In **Web.config**, replace SEARCH_SERVICE_KEY with the query key on the clipboard.

1. Add the following methods to the ```HomeController``` class in **HomeController.cs**:
	
	```C#
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
	```

1.	Use Visual Studio's **Debug -> Start Without Debugging** command (or press **Ctrl+F5**) to launch the application in your browser. 

1. Click **Customer Lookup**. Then type "ar" into the **Search for** box and wait for a list of suggested customers to appear.

    ![Auto-suggest in action](Images/vs-view-autosuggest.png)

    _Auto-suggest in action_

1. Select **Around the Horn** and press **Enter** to search for customers named "Around the Horn."

Auto-suggest vastly improves the search experience and is relatively easy to add thanks to some of the classes you imported in NuGet packages in [Exercise 5](#Exercise5).

<a name="Summary"></a>
## Summary ##

In this hands-on lab you learned how to:

- Create an Azure Cosmos DB account
- Create DocumentDB collections and populate them with documents
- Create an Azure Search service and and use it to index DocumentDB data
- Access DocumentDB collections from your apps
- Query the Azure Search service connected to a DocumentDB database

Not surprisingly, there is much more you can do to leverage the power of the DocumentDB API. Experiment with other DocumentDB features, especially [triggers, stored procedures, and user-defined functions](https://docs.microsoft.com/en-us/azure/documentdb/documentdb-programming), and identify other ways you can enhance your data and search strategies by integrating Azure Cosmos DB and the DocumentDB API into your application ecosystems.

----

Copyright 2017 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.