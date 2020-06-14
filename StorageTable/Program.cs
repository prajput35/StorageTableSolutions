using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageTable
{
    
    class Program
    {
        static string storageconn = "DefaultEndpointsProtocol=https;AccountName=az204300sa;AccountKey=EPU9Oxvr4tG58GukQJcgDs+yxeaaPSv7j+v+pBr8DUzTNScV0JC9LzJOryZgBMvAGDBTm45GM9wGaSUu/jPI2Q==;EndpointSuffix=core.windows.net";
        static string l_table = "Customer";
        static string l_partitionkey = "Architect";
        static string l_rowKey = "userB";
        static void Main(string[] args)
        {
            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageconn);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tblclient.GetTableReference(l_table);

            //InsertTableEntity(table).Wait();

            //ReadEntity(table, l_partitionkey, l_rowKey).Wait();

            //InsertBatch(table).Wait();
            // Query based on partition key
            Query(table).Wait();

            Console.ReadKey();
        }

        static async Task<string> Query(CloudTable p_tbl)
        {

            TableQuery<Customer> CustomerQuery = new TableQuery<Customer>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Azure Developer"
                ));
            var itemlist = p_tbl.ExecuteQuery(CustomerQuery);
            foreach (Customer obj in itemlist)
            {
                Console.WriteLine("The Course Name is " + obj.PartitionKey);
                Console.WriteLine("The User Name is " + obj.RowKey);
                Console.WriteLine("The Price is " + obj.price);
            }
            return "Operation complete";
        }
        public static async Task<string> InsertBatch(CloudTable p_tbl)
        {
            TableBatchOperation l_batch = new TableBatchOperation();
            // All of the records should have the same partition key
            Customer entity1 = new Customer("Azure Developer", "userA");
            entity1.price = 10;
            Customer entity2 = new Customer("Azure Developer", "userB");
            entity2.price = 20;
            Customer entity3 = new Customer("Azure Developer", "userC");
            entity3.price = 30;
            l_batch.Insert(entity1);
            l_batch.Insert(entity2);
            l_batch.Insert(entity3);
            p_tbl.ExecuteBatch(l_batch);
            Console.WriteLine("Records Inserted");
            return "Completed";
        }

        public static async Task<string> InsertTableEntity(CloudTable p_tbl)
        {
            Customer entity = new Customer("Azure Developer", "userD");
            entity.price = 100;
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            TableResult result = await p_tbl.ExecuteAsync(insertOperation);
            Console.WriteLine("Entity Added");
            return "Entity added";
        }

        public static async Task<string> ReadEntity(CloudTable p_tbl,string p_PartitionKey, string p_RowKey)
        {
            TableOperation readOperation = TableOperation.Retrieve<Customer>(p_PartitionKey, p_RowKey);
            TableResult result = await p_tbl.ExecuteAsync(readOperation);
            Customer obj = result.Result as Customer;
            Console.WriteLine("The Course Name is " + obj.PartitionKey);
            Console.WriteLine("The User Name is " + obj.RowKey);
            Console.WriteLine("The Price is " + obj.price);

            return "Entity read operation complete";
        }

    }
}
