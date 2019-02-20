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
            return CosmosWriter.Write(log);
        }
    }

    public static class CosmosWriter
    {
        public static ActionResult Write(ILogger log)
        {
            var server = "https://localhost:8081";
            var databaseName = "CosmosTest";
            var collectionName = "Merchants";
            var key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            
            log.LogInformation("Connecting to {0}. Getting or creating db {1} and getting or creating collection {2}.", server, databaseName, collectionName);
            
            var repo = new CosmosRepository(server, databaseName, collectionName, key);
            
            log.LogInformation("Writing merchant object.");
            var merchant = MerchantFactory.CreateMerchant();
                     
            var doc = repo.CreateDoc(merchant);

            return doc != null
                ? (ActionResult)new OkObjectResult("Wrote Merchant " +  merchant.Id)
                : new BadRequestObjectResult("No document created.");
        }
    }
}
