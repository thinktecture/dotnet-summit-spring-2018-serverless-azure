using Microsoft.Azure.Documents.Client;
using System;
using System.Data.Common;

namespace HomeAutomationFunctions.Data
{
    public static class DocumentClientFactory
    {
        public static DocumentClient Create(string connectionString)
        {
            var builder = new DbConnectionStringBuilder()
            {
                ConnectionString = connectionString
            };

            string authKey;
            Uri serviceEndpoint;

            if (builder.TryGetValue("AccountKey", out var key))
            {
                authKey = key.ToString();
            }
            else
            {
                throw new ArgumentException(
                    "Connectionstring does not contain AccountKey",
                    nameof(connectionString));
            }

            if (builder.TryGetValue("AccountEndpoint", out var uri))
            {
                serviceEndpoint = new Uri(uri.ToString());
            }
            else
            {
                throw new ArgumentException(
                    "Connectionstring does not contain AccountEndpoint",
                    nameof(connectionString));
            }

            return new DocumentClient(serviceEndpoint, authKey);
        }
    }
}
