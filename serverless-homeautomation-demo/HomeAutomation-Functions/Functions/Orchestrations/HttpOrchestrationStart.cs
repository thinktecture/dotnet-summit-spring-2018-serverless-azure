using HomeAutomation.Common;
using HomeAutomationFunctions.Security;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HomeAutomationFunctions.Functions.Orchestrations
{
    public static class HttpOrchestrationStart
    {
        [FunctionName(MethodNames.HttpOrchestrationStart)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, methods: "post", Route = "orchestrators/{functionName}")]
            HttpRequestMessage req,
            [OrchestrationClient]
            DurableOrchestrationClient starter,
            string functionName,
            TraceWriter log)
        {
            if (!await req.CheckAuthorization("api"))
            {
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            dynamic eventData = await req.Content.ReadAsAsync<object>();
            string instanceId = await starter.StartNewAsync(functionName, eventData);

            log.Info($"Started orchestration with ID = '{instanceId}'.");

            var response = starter.CreateCheckStatusResponse(req, instanceId);
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(10));

            return response;
        }
    }
}
