using HomeAutomation.Common;
using HomeAutomationFunctions.Data;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio;

namespace HomeAutomationFunctions.Functions
{
    public static class NotifyHighTemperaturesBySms
    {
        [FunctionName(MethodNames.NotifyHighTemperaturesBySms)]
        public static async Task Run(
            [CosmosDBTrigger("DeviceData", "Climate", ConnectionStringSetting ="CosmosDbConnectionString", CreateLeaseCollectionIfNotExists =true)]
            IReadOnlyList<Document> documents,
            [TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "%TwilioFromNumber%")]
            IAsyncCollector<SMSMessage> sms,
            ILogger log)
        {
            log.LogInformation("Function triggered by CosmosDB...");

            var message = new SMSMessage();

            if (documents == null || documents.Count <= 0)
            {
                return;
            }

            foreach (var document in documents)
            {
                var temperature = document.GetPropertyValue<int>("temperature");

                if (temperature > 30)
                {
                    log.LogInformation("###WARNING! Temperature: " + temperature);

                    message.Body = $"WARNING - high temperature!!! {temperature}°";
                    // TODO: get from database & loop through receivers
                    message.To = "+49 175 2914416";
                    await sms.AddAsync(message);

                    var events = new List<Event>
                        {
                            new Event
                            {
                                EventTime = DateTime.UtcNow,
                                EventType = "temperatureTooHigh",
                                Subject = "thinktecture/homeautomation",
                                Id = Guid.NewGuid().ToString(),
                                Data = new Alert {Message = "High temperature: " + temperature}
                            }
                        };

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("aeg-sas-key", Environment.GetEnvironmentVariable("EventGridSasToken"));
                    await client.PostAsJsonAsync(Environment.GetEnvironmentVariable("EventGridEventsEndpoint"), events);
                }
            }
        }
    }
}
