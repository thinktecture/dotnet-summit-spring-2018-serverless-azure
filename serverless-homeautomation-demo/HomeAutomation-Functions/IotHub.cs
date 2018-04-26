using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace HomeAutomationFunctions.Functions
{
    public static class IotHub
    {
        private static ServiceClient _client;
        private static ServiceClient GetServiceClient()
        {
            if (_client != null)
            {
                return _client;
            }
            var connectionString = Environment.GetEnvironmentVariable("IotHubConnectionString");
            return _client = ServiceClient.CreateFromConnectionString(connectionString);
        }

        public static async Task<(TResult, CloudToDeviceMethodResult)> InvokeDeviceMethodAsync<TPayload, TResult>(
            string deviceId,
            string methodName,
            TPayload payload)
        {
            var response = await InvokeDeviceMethodAsync(deviceId, methodName, payload);
            return (JsonConvert.DeserializeObject<TResult>(response.GetPayloadAsJson()), response);
        }

        public static Task<CloudToDeviceMethodResult> InvokeDeviceMethodAsync<TPayload>(
            string deviceId,
            string methodName,
            TPayload payload)
        {
            var method = new CloudToDeviceMethod(methodName);
            method.SetPayloadJson(JsonConvert.SerializeObject(payload));

            var serviceClient = GetServiceClient();
            return serviceClient.InvokeDeviceMethodAsync(deviceId, method);
        }

        public static async Task<(TResult, CloudToDeviceMethodResult)> InvokeDeviceMethodAsync<TResult>(
          string deviceId,
          string methodName)
        {
            var method = new CloudToDeviceMethod(methodName);

            var serviceClient = GetServiceClient();
            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, method);

            return (JsonConvert.DeserializeObject<TResult>(response.GetPayloadAsJson()), response);
        }
    }
}
