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
    public static class WriteToSql
    {
        [FunctionName("WriteToSql")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return SqlWriter.Write(log);
        }
    }

    public static class SqlWriter
    {
        public static IActionResult Write(ILogger log)
        {
            var connectionString = "Data Source=localhost;Initial Catalog=Merchant;User ID=sa;Password=P@ssw0rd;Connection Timeout=25;";
            
            log.LogInformation("Using connection string -> {0}", connectionString);
            
            var repo = new SqlRepository(connectionString);
            
            var merchant = MerchantFactory.CreateMerchant();
            log.LogInformation("Writing merchant object with ID {0}.", merchant.Id);

            var message = "";
            Merchant doc = null;
            try
            {
              doc = repo.CreateMerchant(merchant);
              message = "Wrote Merchant " + merchant.Id;
            }
            catch(Exception e)
            {
                message = "Failed due to " + e.Message;
            }

            return doc != null
                ? (ActionResult)new OkObjectResult(message)
                : new BadRequestObjectResult(message);
        }
    }
}
