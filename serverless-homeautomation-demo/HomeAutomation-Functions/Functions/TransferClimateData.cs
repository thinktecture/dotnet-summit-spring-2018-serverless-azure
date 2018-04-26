using HomeAutomation.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace HomeAutomationFunctions.Functions
{
    public static class TransferClimateData
    {
        [FunctionName(MethodNames.TransferClimateData)]
        public static void Run(
            [EventHubTrigger("%EventHubName%", Connection = "EventHubConnectionString")]
            dynamic eventHubMessage,
            [DocumentDB("%CosmosDbNameDeviceData%", "%CosmosDbCollectionClimateData%", ConnectionStringSetting = "CosmosDbConnectionString")]
            out dynamic document,
            ILogger log)
        {
            log.LogInformation($"C# Event Hub trigger function processed a message: {eventHubMessage}");

            document = new
            {
                eventHubMessage.messageId,
                eventHubMessage.deviceId,
                eventHubMessage.temperature,
                eventHubMessage.humidity
            };
        }
    }
}
