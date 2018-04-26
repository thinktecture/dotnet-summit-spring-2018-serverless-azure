using HomeAutomation.Common;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using HomeAutomationFunctions.Notifications;

namespace HomeAutomationFunctions.Functions
{
    public static class HttpSoftwareUpdateNotification
    {
        [FunctionName(MethodNames.NotifySoftwareUpdateTriggeredByHttp)]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequestMessage req,
            [NotificationHub(ConnectionStringSetting = "NotificationHubConnectionString", HubName = "%NotificationHubName%")]
            IAsyncCollector<Notification> notification,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            const string message = "A new software update is available.";

            NotificationPayloadBase payload = new GcmNotificationPayload(message);
            await notification.AddAsync(new GcmNotification(JsonConvert.SerializeObject(payload)));

            payload = new AppleNotificationPayload(message);
            await notification.AddAsync(new AppleNotification(JsonConvert.SerializeObject(payload)));
        }
    }
}
