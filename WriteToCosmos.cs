using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace AzureDatabaseComparison
{
    public static class WriteToCosmos
    {
        [FunctionName("WriteToCosmos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string cosmosConnectionUri = "https://localhost:8081";
            string databaseName = "CosmosTest";
            string merchantCollectionName = "Merchants";
            
            // connect to CosmosDB
            log.LogInformation("Connecting to {0}.", cosmosConnectionUri);
            DocumentClient cosmosClient = new DocumentClient(new Uri(cosmosConnectionUri), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            // get database and collection
            log.LogInformation("Get database {0} and collection {1}.", databaseName, merchantCollectionName);
            var db = GetOrCreateDatabaseAsync(cosmosClient, databaseName);
            var collection = GetOrCreateCollectionAsync(cosmosClient, databaseName, merchantCollectionName);
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, merchantCollectionName);

            // create document
            log.LogInformation("Writing merchant object.");
            var merchant = new Merchant {
                Id = "1",
                Name = "Test",
                Ein = "12-3456789",
                YearlyVolume = 22222.22m,
                AverageTicket = 22.22m,
                ContactName = "Mr. Me"
            };
            
            var doc = await cosmosClient.CreateDocumentAsync(collectionLink, merchant);

            return doc != null
                ? (ActionResult)new OkObjectResult("Done")
                : new BadRequestObjectResult("No document created.");
        }

        // ***********************************************************************
        // Below are functions swiped from CosmosDB C# sample (CosmosSample.cs)
        // ***********************************************************************
        
        private static async Task<DocumentCollection> GetOrCreateCollectionAsync(DocumentClient client, string databaseId, string collectionId)
        {
            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);

            DocumentCollection collection = client.CreateDocumentCollectionQuery(databaseUri)
                .Where(c => c.Id == collectionId)
                .AsEnumerable()
                .FirstOrDefault();

            if (collection == null)
            {
                collection = await client.CreateDocumentCollectionAsync(databaseUri, new DocumentCollection { Id = collectionId });
            }

            return collection;
        }

        private static async Task<Database> GetOrCreateDatabaseAsync(DocumentClient client, string databaseId)
        {
            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);

            Database database = client.CreateDatabaseQuery()
                .Where(db => db.Id == databaseId)
                .ToArray()
                .FirstOrDefault();

            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = databaseId });
            }

            return database;
        }
    }
}
