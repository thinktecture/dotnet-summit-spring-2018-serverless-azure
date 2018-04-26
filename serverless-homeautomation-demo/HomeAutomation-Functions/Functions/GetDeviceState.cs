using HomeAutomation.Common;
using HomeAutomationFunctions.Infrastructure;
using HomeAutomationFunctions.Security;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAutomationFunctions.Functions
{
    public static class GetDeviceState
    {
        [FunctionName(MethodNames.GetDeviceState)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devicestate/{hubDeviceId}/{targetDevice}")]
            HttpRequestMessage req,
            string hubDeviceId,
            string targetDevice,
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
            var request = new GetDeviceStateRequest() { Device = targetDevice };
            var (result, response) = await IotHub.InvokeDeviceMethodAsync<GetDeviceStateRequest, GetDeviceStateResponse>(
                hubDeviceId,
                MethodNames.GetDeviceState,
                request);

            return req.CreateResponse((HttpStatusCode)response.Status, result, ResultFormatter.GetFormatter());
        }
    }
}
