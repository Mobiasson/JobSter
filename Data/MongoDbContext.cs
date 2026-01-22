using MongoDB.Driver;
using JobSter.Model;

namespace JobSter.Data {
    public class MongoDbContext {
        private readonly MongoClient _client;
        public IMongoDatabase Database { get; }

        private const string DatabaseName = "MikaelRosTobiasson";

        public IMongoCollection<Company> Companies => Database.GetCollection<Company>("companies");
        public IMongoCollection<JobApplication> JobApplications => Database.GetCollection<JobApplication>("jobApplications");

        public MongoDbContext(string? connectionString = null) {
            connectionString ??= "mongodb://localhost:27017/";
            _client = new MongoClient(connectionString);
            Database = _client.GetDatabase(DatabaseName);
        }

        public System.Threading.Tasks.Task EnsureSeedDataAsync() => DbSeeder.SeedIfEmptyAsync(this);
    }
}
