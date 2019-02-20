using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace AzureDatabaseComparison
{
    public static class CompareDatabases
    {
        [FunctionName("CompareDatabases")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // log.LogInformation("Using connection string -> {0}", connectionString);
            

            // log.LogInformation("Writing merchant object with ID {0}.", merchant.Id);

            // int numberToTest = 100;
            
            // // SQL test
            // var sqlTestStart = DateTime.Now;
            // for(int i = 0; i < numberToTest; i++)
            // {
            //     var response = await WriteToSql.
            // }
            // var sqlTestEnd = DateTime.Now;
            

            // // Cosmos Test


            // return doc != null
            //     ? (ActionResult)new OkObjectResult(message)
            //     : new BadRequestObjectResult(message);
            return (ActionResult) new OkObjectResult("test");
        }

        
    }

    public class TestResults
    {
        public string Results {get; set;}

        public TestResults(DateTime start, DateTime end, int totalCount, int success, int fail)
        {
            var duration = end.Subtract(start);
            Results = string.Format("Ran {0} in {1} sec.", totalCount, duration.Seconds);
        }
    }
}
