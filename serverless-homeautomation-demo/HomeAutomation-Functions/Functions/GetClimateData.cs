using HomeAutomation.Common;
using HomeAutomationFunctions.Data;
using HomeAutomationFunctions.Security;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ClimateData = HomeAutomationFunctions.Data.ClimateData;

namespace HomeAutomationFunctions.Functions
{
    public static class GetClimateData
    {
        [FunctionName(MethodNames.GetClimateData)]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "climate/data")]
            HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("QueryClimateData HTTP trigger function processed a request.");

            if (!await req.CheckAuthorization("api"))
            {
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            var climateData = await QueryClimateData();
            var result = climateData.ToList();

            return req.CreateResponse(HttpStatusCode.OK, result, "application/json");
        }

        private static Task<FeedResponse<ClimateData>> QueryClimateData()
        {
            var client = DocumentClientFactory.Create(
                Environment.GetEnvironmentVariable("CosmosDbConnectionString"));

            var queryOptions = new FeedOptions { MaxItemCount = 30, EnableCrossPartitionQuery = true };
            IDocumentQuery<ClimateData> dataQuery =
                client.CreateDocumentQuery<ClimateData>(
                    UriFactory.CreateDocumentCollectionUri(
                        Environment.GetEnvironmentVariable("CosmosDbNameDeviceData"),
                        Environment.GetEnvironmentVariable("CosmosDbCollectionClimateData")),
                    queryOptions).OrderByDescending(d => d.Timestamp).AsDocumentQuery();

            return dataQuery.ExecuteNextAsync<ClimateData>();
        }
    }
}

