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
            int numberToTest = 100;
            
            // SQL test
            log.LogInformation("Starting SQL test.");
            int sqlSuccess = 0;
            int sqlFailure = 0;
            var sqlTestStart = DateTime.Now;
            for(int i = 0; i < numberToTest; i++)
            {
                var response = SqlWriter.Write(log);
                if(response.GetType() == typeof(OkObjectResult))
                {
                    sqlSuccess++;
                }
                else
                {
                    sqlFailure++;
                }
            }
            var sqlTestEnd = DateTime.Now;
            var sqlResults = new TestResults(sqlTestStart, sqlTestEnd, numberToTest, sqlSuccess, sqlFailure, "sql");

            log.LogInformation("Starting Cosmos test.");
            // Cosmos Test
            int cosmosSuccess = 0;
            int cosmosFailure = 0;
            var cosmosTestStart = DateTime.Now;
            for(int i = 0; i < numberToTest; i++)
            {
                var response = CosmosWriter.Write(log);
                if(response.GetType() == typeof(OkObjectResult))
                {
                    cosmosSuccess++;
                }
                else
                {
                    cosmosFailure++;
                }
            }
            var cosmosTestEnd = DateTime.Now;
            var cosmosResults = new TestResults(cosmosTestStart, cosmosTestEnd, numberToTest, cosmosSuccess, cosmosFailure, "cosmos");

            log.LogInformation(sqlResults.Results);
            log.LogInformation(cosmosResults.Results);
            
            return (ActionResult)new OkObjectResult(sqlResults.Results + "\n" + cosmosResults.Results);
        }

        
    }

    public class TestResults
    {
        public string Results {get; set;}

        public TestResults(DateTime start, DateTime end, int totalCount, int success, int fail, string name)
        {
            var duration = end.Subtract(start);
            Results = string.Format("Ran {4} {0} with {2} successes and {3} failures in {1} sec.", totalCount, duration.Seconds, success, fail, name);
        }
    }
}
