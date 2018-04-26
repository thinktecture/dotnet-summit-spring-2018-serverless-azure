using Microsoft.WindowsAzure.Storage.Table;

namespace UrlManager1
{
    public class UrlKey : TableEntity
    {
        public int Id { get; set; }
    }
}
