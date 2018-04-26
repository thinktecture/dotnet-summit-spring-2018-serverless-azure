using Microsoft.WindowsAzure.Storage.Table;

namespace UrlManager1
{
    public class UrlData : TableEntity
    {
        public string Url { get; set; }
        public int Count { get; set; }
    }
}
