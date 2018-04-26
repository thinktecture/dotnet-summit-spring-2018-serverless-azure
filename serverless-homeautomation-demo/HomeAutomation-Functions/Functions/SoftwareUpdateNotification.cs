using HomeAutomation.Common;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HomeAutomationFunctions.Notifications;

namespace HomeAutomationFunctions.Functions
{
    public static class SoftwareUpdateNotificationStorage
    {
        [FunctionName(MethodNames.SoftwareUpdateNotification)]
        public static async Task SoftwareUpdateNotification(
            [EventGridTrigger]
            EventGridEvent eventGridEvent,
            [NotificationHub(ConnectionStringSetting = "NotificationHubConnectionString", HubName = "%NotificationHubName%")]
            IAsyncCollector<Notification> notification,
            ILogger log)
        {
            log.LogInformation("EventGridEvent trigger." +
                $"\n\tId:{eventGridEvent.Id}" +
                $"\n\tTopic:{eventGridEvent.Topic}" +
                $"\n\tSubject:{eventGridEvent.Subject}" +
                $"\n\tType:{eventGridEvent.EventType}" +
                $"\n\tData:{JsonConvert.SerializeObject(eventGridEvent.Data)}");

            // TODO: check event grid data for correct data - and use file version info to embed it in the push payload
            const string message = "A new software update is available.";

            NotificationPayloadBase payload = new GcmNotificationPayload(message);
            await notification.AddAsync(new GcmNotification(JsonConvert.SerializeObject(payload)));

            payload = new AppleNotificationPayload(message);
            await notification.AddAsync(new AppleNotification(JsonConvert.SerializeObject(payload)));
        }
    }
}
