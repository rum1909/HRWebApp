//using MongoDB.Driver;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace HRWebApp.Models
//{

//    public class DataAccessLayer
//    {
//        private readonly IMongoCollection<MongoDBPersonal> _personCollection;

//        public DataAccessLayer(string connectionString, string databaseName)
//        {
//            var client = new MongoClient(connectionString);
//            var database = client.GetDatabase(databaseName);
//            _personCollection = database.GetCollection<MongoDBPersonal>("Persons");
//        }

//        public async Task<List<MongoDBPersonal>> GetAllPersonsAsync()
//        {
//            return await _personCollection.Find(_ => true).ToListAsync();
//        }

//        public async Task<MongoDBPersonal> GetPersonByIdAsync(string id)
//        {
//            return await _personCollection.Find(p => p.employeeId == id).FirstOrDefaultAsync();
//        }

//        public async Task AddPersonAsync(MongoDBPersonal person)
//        {
//            await _personCollection.InsertOneAsync(person);
//        }

//        public async Task<bool> UpdatePersonAsync(string id, MongoDBPersonal person)
//        {
//            var result = await _personCollection.ReplaceOneAsync(p => p.employeeId == id, person);
//            return result.IsAcknowledged && result.ModifiedCount > 0;
//        }

//        public async Task<bool> DeletePersonAsync(string id)
//        {
//            var result = await _personCollection.DeleteOneAsync(p => p.employeeId == id);
//            return result.IsAcknowledged && result.DeletedCount > 0;
//        }
//    }
//}
