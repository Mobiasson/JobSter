using System;
using System.Windows;
using JobSter.Services;

namespace JobSter
{
    public partial class App : Application
    {
        public static MongoDbService? MongoDb { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                MongoDb = new MongoDbService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize MongoDB service.\n{ex.Message}", "Startup error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(-1);
            }
        }
    }
}
