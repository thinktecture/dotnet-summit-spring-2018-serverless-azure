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
    public static class GetBuildingInfo
    {
        [FunctionName(MethodNames.GetBuildingInfo)]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "buildinginfo/{hubDeviceId}")]
            HttpRequestMessage req,
            string hubDeviceId,
            ILogger log)
        {
            log.LogInformation("GetBuildingInfo HTTP trigger function processed a request.");

            if (!await req.CheckAuthorization("api"))
            {
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (String.IsNullOrWhiteSpace(hubDeviceId))
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown device");
            }

            var (result, response) = await IotHub.InvokeDeviceMethodAsync<GetBuildingInfoResponse>(hubDeviceId, MethodNames.GetBuildingInfo);

            return req.CreateResponse((HttpStatusCode)response.Status, result, ResultFormatter.GetFormatter());
        }
    }
}

