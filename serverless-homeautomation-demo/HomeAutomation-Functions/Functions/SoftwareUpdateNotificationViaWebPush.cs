using HomeAutomation.Common;
using HomeAutomationFunctions.Data;
using HomeAutomationFunctions.Functions.Data;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebPush;

namespace HomeAutomationFunctions.Functions
{
    public static class SoftwareUpdateNotificationViaWebPush
    {
        // TODO: move into Key Vault!
        private static readonly string PublicKey = "BPwp3UQzC-qHcdQYTPvmVRYG7QFTg24H6ifVYeUf51U8G-_rqwP-bEmvmS9k7NS8qsS7l7qiteiB_ZSTiF5VV9c";
        private static readonly string PrivateKey = "A17zC24Wuy1pvAybijGoHQ3JlfmidlUe4yREuor-kz0";

        private static readonly string Database = Environment.GetEnvironmentVariable("CosmosDbNameInfrastructureData");
        private static readonly string Collection = Environment.GetEnvironmentVariable("CosmosDbCollectionPushSubscriptions");
        private static readonly string SprocName = Environment.GetEnvironmentVariable("CosmosDbDeleteSubscriptionSprocName");
        private static DocumentClient _documentClient;

        [FunctionName(MethodNames.NotifySoftwareUpdateViaWebPush)]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // TODO: check token
            await DoWebPush(log);
        }

        private static async Task DoWebPush(ILogger log)
        {
            var subject = @"mailto:azure@thinktecture.com";
            var subscriptions = await GetSubscriptions();

            foreach (var subscription in subscriptions)
            {
                var vapidDetails = new VapidDetails(subject, PublicKey, PrivateKey);
                var webPushClient = new WebPushClient();
                PushData pushData = CreatePushMessage();

                try
                {
                    webPushClient.SendNotification(subscription, JsonConvert.SerializeObject(pushData), vapidDetails);
                }
                catch (AggregateException exception)
                {
                    log.LogError("Error when pushing: " + exception.StackTrace);

                    if (exception.InnerException is WebPushException)
                    {
                        await _documentClient.ExecuteStoredProcedureAsync<dynamic>(
                            UriFactory.CreateStoredProcedureUri(Database, Collection, SprocName), subscription.Endpoint);
                    }
                }
            }
        }

        private static PushData CreatePushMessage()
        {
            var actions = new List<ActionData> { new ActionData { Action = "showdetails", Title = "Show details..." } };
            var pushData = new PushData
            {
                Notification = new Notification
                {
                    Title = "Update",
                    Body = "New software available!",
                    Icon = "/assets/icon-192x192.png",
                    Badge = "/assets/icon-96x96.png",
                    Actions = actions
                }
            };
            return pushData;
        }

        private static void InitializeDocumentClient()
        {
            _documentClient = DocumentClientFactory.Create(
                Environment.GetEnvironmentVariable("CosmosDbConnectionString"));
        }

        private static async Task<FeedResponse<PushSubscription>> GetSubscriptions()
        {
            InitializeDocumentClient();

            IDocumentQuery<PushSubscription> dataQuery =
                            _documentClient.CreateDocumentQuery<PushSubscription>(
                                UriFactory.CreateDocumentCollectionUri(Database, Collection))
                                .AsDocumentQuery();

            return await dataQuery.ExecuteNextAsync<PushSubscription>();
        }
    }
}
