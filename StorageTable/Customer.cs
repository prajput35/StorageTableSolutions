
using Microsoft.Azure.Cosmos.Table;

namespace StorageTable
{
    class Customer : TableEntity
    {
        public Customer() { }

        public Customer(string coursename, string username)
        {
            PartitionKey = coursename;
            RowKey = username;
        }
        public int price { get; set; }
    }
}
