using System;
using System.Configuration;
using HRWebApp.Models;
using MongoDB.Driver;

public class MongoDBTest
{
    public static void TestConnectionAndQuery()
    {
        try
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["apicompany"];

            var client = new MongoClient(connectionString); // Kết nối đến MongoDB
            if (client == null)
            {
                Console.WriteLine("Failed to connect to MongoDB client.");
                return;
            }

            var database = client.GetDatabase(databaseName); // Chọn database trong MongoDB
            if (database == null)
            {
                Console.WriteLine("Failed to get MongoDB database.");
                return;
            }

            var collection = database.GetCollection<MongoDBPersonal>("employees"); // Chọn collection trong database
            if (collection == null)
            {
                Console.WriteLine("Failed to get MongoDB collection.");
                return;
            }

            // Thực hiện một truy vấn đơn giản để lấy một tài liệu từ collection và kiểm tra xem kết quả trả về có dữ liệu hay không
            var firstDocument = collection.Find(_ => true).FirstOrDefault();
            if (firstDocument == null)
            {
                Console.WriteLine("No data found in MongoDB collection.");
                return;
            }

            // In ra thông tin của tài liệu đầu tiên
            Console.WriteLine("First document in MongoDB collection: " + firstDocument.ToString());
        }
        catch (MongoException mongoEx)
        {
            Console.WriteLine("MongoDB connection failed. Exception: " + mongoEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred. Exception: " + ex.Message);
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        // Gọi method TestConnectionAndQuery để kiểm tra kết nối và lấy dữ liệu từ MongoDB collection
        MongoDBTest.TestConnectionAndQuery();
    }
}

