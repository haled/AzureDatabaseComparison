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
            log.LogInformation("Connecting to {0}.", "https://localhost:8081");
            
            var repo = new CosmosRepository("https://localhost:8081","CosmosTest","Merchants", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            
            // get database and collection
//            log.LogInformation("Get database {0} and collection {1}.", databaseName, merchantCollectionName);
            

            // create document
            log.LogInformation("Writing merchant object.");
            var merchant = MerchantFactory.CreateMerchant();
                     
            //var doc = await cosmosClient.CreateDocumentAsync(collectionLink, merchant);
            var doc = repo.CreateDoc(merchant);

            return doc != null
                ? (ActionResult)new OkObjectResult("Wrote Merchant " +  merchant.Id)
                : new BadRequestObjectResult("No document created.");
        }

        
    }
}
