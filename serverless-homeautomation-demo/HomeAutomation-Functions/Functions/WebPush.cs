using HomeAutomation.Common;
using HomeAutomationFunctions.Data;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebPush;

namespace HomeAutomationFunctions.Functions
{
    public static class WebPush
    {
        private static readonly string Database = Environment.GetEnvironmentVariable("CosmosDbNameInfrastructureData");
        private static readonly string Collection = Environment.GetEnvironmentVariable("CosmosDbCollectionPushSubscriptions");
        private static readonly string SprocName = Environment.GetEnvironmentVariable("CosmosDbDeleteSubscriptionSprocName");
        private static DocumentClient _documentClient;

        [FunctionName(MethodNames.WebPush)]
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // TODO: check token

            InitializeDocumentClient();

            var body = req.Content.ReadAsStringAsync().Result;
            var request = JsonConvert.DeserializeObject<dynamic>(body);

            if (request.action == "subscribe")
            {
                RegisterPushSubscription(request);
            }
            else if (request.action == "unsubscribe")
            {
                UnregisterPushSubscription(request);
            }
            else
            {
                req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }
        }

        private static async Task RegisterPushSubscription(dynamic request)
        {
            PushSubscription incomingSubscription = BuildSubscriptionFromRequest(request);
            PushSubscription storedSubscription = await SearchForSubscription(incomingSubscription);

            if (storedSubscription == null)
            {
                await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Database, Collection), incomingSubscription);
            }
        }

        private static async Task UnregisterPushSubscription(dynamic request)
        {
            PushSubscription incomingSubscription = BuildSubscriptionFromRequest(request);
            PushSubscription storedSubscription = await SearchForSubscription(incomingSubscription);

            if (storedSubscription != null)
            {
                await _documentClient.ExecuteStoredProcedureAsync<dynamic>(
                            UriFactory.CreateStoredProcedureUri(Database, Collection, SprocName), storedSubscription.Endpoint);
            }
        }

        private static void InitializeDocumentClient() =>
            _documentClient = DocumentClientFactory.Create(Environment.GetEnvironmentVariable("CosmosDbConnectionString"));

        private static PushSubscription BuildSubscriptionFromRequest(dynamic request)
        {
            return new PushSubscription
            {
                Endpoint = request.subscription.endpoint,
                Auth = request.subscription.keys.auth,
                P256DH = request.subscription.keys.p256dh
            };
        }

        private static async Task<PushSubscription> SearchForSubscription(PushSubscription subscription)
        {
            IDocumentQuery<PushSubscription> dataQuery =
                            _documentClient.CreateDocumentQuery<PushSubscription>(
                                UriFactory.CreateDocumentCollectionUri(Database, Collection))
                                .Where(sub => sub.Endpoint == subscription.Endpoint).AsDocumentQuery();

            var feedResponse = await dataQuery.ExecuteNextAsync<PushSubscription>();

            return feedResponse.FirstOrDefault();
        }
    }
}
