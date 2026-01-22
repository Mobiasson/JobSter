using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace JobSter.Services;
public class MongoDbService {
    private static readonly string _connectionString = "mongodb://localhost:27017/";
    public MongoDbService() {
        var client = new MongoClient(_connectionString);
        var myUser = new User() { FirstName = "Fredrik", LastName = "Johansson" };

        var userCollection = client.GetDatabase("MikaelRosTobiasson").GetCollection<User>("users");

        //userCollection.InsertOne(myUser);

        var userFilter = Builders<User>.Filter.Eq("FirstName", "Fredrik");
        var userUpdate = Builders<User>.Update.Set("LastName", "Gustavsson");

        //userCollection.UpdateMany(userFilter, userUpdate);

        userCollection.DeleteMany(userFilter);
    }

}
class User {
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
