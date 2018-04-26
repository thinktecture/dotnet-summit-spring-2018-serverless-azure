using HomeAutomation.Common;
using HomeAutomationFunctions.Security;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAutomationFunctions.Functions
{
    public static class ControlClimateData
    {
        private static readonly HashSet<string> _commands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "start", "stop"
        };

        [FunctionName(MethodNames.ControlClimateData)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "climate/{hubDeviceId}/{command}")]
            HttpRequestMessage req,
            string hubDeviceId,
            string command,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!await req.CheckAuthorization("api"))
            {
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (String.IsNullOrWhiteSpace(hubDeviceId))
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown device");
            }

            if (String.IsNullOrWhiteSpace(command) || !_commands.Contains(command))
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown command");
            }

            var request = new ControlClimateDataRequest() { Device = hubDeviceId, Command = command };
            var response = await IotHub.InvokeDeviceMethodAsync(hubDeviceId, MethodNames.ControlClimateData, request);

            return req.CreateResponse((HttpStatusCode)response.Status, "Command sent: " + command, "application/json");
        }
    }
}
