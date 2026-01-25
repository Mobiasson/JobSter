using JobSter.Model;
using MongoDB.Driver;

namespace JobSter.Services;

public class MongoDbService {
    private static readonly string _connectionString = "mongodb://localhost:27017/";

    public IMongoCollection<Company> Companies { get; }
    public IMongoCollection<JobApplication> JobApplications { get; }
    public IMongoCollection<User> Users { get; }

    public MongoDbService() {
        var client = new MongoClient(_connectionString);
        var db = client.GetDatabase("MikaelRosTobiasson");

        Companies = db.GetCollection<Company>("companies");
        JobApplications = db.GetCollection<JobApplication>("jobApplications");
        Users = db.GetCollection<User>("users");
        if(JobApplications.CountDocuments(Builders<JobApplication>.Filter.Empty) == 0) {
            var demoJob = new JobApplication {
                Title = "Test Developer",
                CompanyName = "Demo AB",
                AppliedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            JobApplications.InsertOne(demoJob);
            System.Windows.MessageBox.Show("Demo data inserted – check Compass now!");
        }
    }

    public void CreateUser(User user) {
        Users.InsertOne(user);
    }

    public User? GetUserByUsername(string username) {
        return Users.Find(u => u.Username == username).FirstOrDefault();
    }

    public User? GetUserByUsernameAndPassword(string username, string password) {
        return Users.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
    }
}