using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace HomeAutomationFunctions.Infrastructure
{
    internal static class ResultFormatter
    {
        public static JsonMediaTypeFormatter GetFormatter()
        {
            var mtf = new JsonMediaTypeFormatter
            {
                SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            };

            return mtf;
        }
    }
}
