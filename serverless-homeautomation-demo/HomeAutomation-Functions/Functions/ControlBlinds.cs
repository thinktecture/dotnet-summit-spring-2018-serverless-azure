using HomeAutomation.Common;
using HomeAutomationFunctions.Infrastructure;
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
    public static class ControlBlinds
    {
        private static readonly HashSet<string> _commands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "up", "down", "stop"
        };

        [FunctionName(MethodNames.ControlBlinds)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "blinds/{hubDeviceId}/{targetDevice}/{command}")]
            HttpRequestMessage req,
            string hubDeviceId,
            string targetDevice, //REVIEW: Should we validate target devices?
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

            var request = new ControlBlindsRequest() { Device = targetDevice, Command = command };
            var (result, response) = await IotHub.InvokeDeviceMethodAsync<ControlBlindsRequest, ControlBlindsResponse>(
                hubDeviceId,
                MethodNames.ControlBlinds,
                request);

            return req.CreateResponse((HttpStatusCode)response.Status, result, ResultFormatter.GetFormatter());
        }
    }
}
