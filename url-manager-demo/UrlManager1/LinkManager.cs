using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace UrlManager1
{
    public static class LinkManager
    {
        [FunctionName("add")]
        public static async Task<HttpResponseMessage> AddUrl(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequestMessage req,
            [Table("urls", "1", "KEY", Take = 1)]
            UrlKey keyTable,
            [Table("urls")]
            CloudTable outTable,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string url = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "url", true) == 0)
                .Value;

            if (url == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                url = data?.url;
            }

            if (keyTable == null)
            {
                keyTable = new UrlKey
                {
                    PartitionKey = "1",
                    RowKey = "KEY",
                    Id = 1024
                };
                var addKey = TableOperation.Insert(keyTable);
                await outTable.ExecuteAsync(addKey);
            }

            int idx = keyTable.Id;
            const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var s = string.Empty;

            while (idx > 0)
            {
                s += ALPHABET[idx % ALPHABET.Length];
                idx /= ALPHABET.Length;
            }

            var code = string.Join(string.Empty, s.Reverse());

            var urlData = new UrlData
            {
                PartitionKey = $"{code[0]}",
                RowKey = code,
                Url = url,
                Count = 1
            };

            keyTable.Id++;

            var operation = TableOperation.Replace(keyTable);
            await outTable.ExecuteAsync(operation);
            operation = TableOperation.Insert(urlData);
            await outTable.ExecuteAsync(operation);

            return req.CreateResponse(HttpStatusCode.OK, urlData.RowKey);
        }

        [FunctionName("go")]
        public static async Task<HttpResponseMessage> RedirectToUrl(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Go/{shortUrl}")]
            HttpRequestMessage req,
            [Table("urls")]
            CloudTable inputTable,
            string shortUrl,
            [Queue(queueName: "counts")]
            IAsyncCollector<string> queue,
            TraceWriter log)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            shortUrl = shortUrl.ToUpper();

            var operation = TableOperation.Retrieve<UrlData>(
                shortUrl[0].ToString(), shortUrl);
            var result = await inputTable.ExecuteAsync(operation);

            var url = "https://www.thinktecture.com";

            if (result != null && result.Result is UrlData data)
            {
                url = data.Url;
                await queue.AddAsync(data.RowKey);
            }

            var response = req.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Add("Location", url);

            return response;
        }

        [FunctionName("ProcessCounts")]
        public static async Task ProcessQueue(
            [QueueTrigger("counts")]
            string shortCode,
            [Table("urls")]
            CloudTable inputTable,
            TraceWriter log)
        {
            var operation = TableOperation.Retrieve<UrlData>(
                shortCode[0].ToString(), shortCode);

            var result = await inputTable.ExecuteAsync(operation);

            if (result != null && result.Result is UrlData data)
            {
                data.Count++;
                operation = TableOperation.Replace(data);

                await inputTable.ExecuteAsync(operation);
            }
        }
    }
}
