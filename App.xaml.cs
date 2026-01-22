using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using JobSter.Data;

public partial class App : Application {
    public static IConfigurationRoot Configuration { get; private set; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    protected override async void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        var conn = Configuration["Mongo:ConnectionString"];
        var dbName = Configuration["Mongo:Database"];

        // If you update MongoDbContext to accept a database name, pass dbName; otherwise keep DB name in the class.
        var ctx = new MongoDbContext(conn);

        await ctx.EnsureSeedDataAsync();

        // Create your MainViewModel and pass ctx or services that use it, then show MainWindow
    }
}
