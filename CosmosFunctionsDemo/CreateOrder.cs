using System;
using System.Collections.Generic;
using System.IO;
using CosmosFunctionsDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CosmosFunctionsDemo
{
    public static class CreateOrder
    {
        [FunctionName("CreateOrder")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [CosmosDB(databaseName: "%cosmosDatabaseName%",
                collectionName: "%ordersContainerName%",
                ConnectionStringSetting = "cosmosConnectionString")] out Order orderToSave)
        {
            // Can't use out parameters with async functions, so if we want to use an out param,
            // we have to fall back to synchronous code to read request body
            using (var reader = new StreamReader(req.Body))
            {
                var orderString = reader.ReadToEnd();
                var order = JsonConvert.DeserializeObject<Order>(orderString);
                orderToSave = order;
            }
            
            return new NoContentResult();
        }
    }
}
