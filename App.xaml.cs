using System;
using System.Windows;
using JobSter.Model;
using JobSter.Services;
using JobSter.Views;

namespace JobSter {
    public partial class App : Application {
        public static MongoDbService? MongoDb { get; private set; }
        public static User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            try {
                MongoDb = new MongoDbService();
                var loginView = new LoginView();
                MainWindow = loginView;
                loginView.Show();
            }
            catch(Exception ex) {
                MessageBox.Show($"Failed to initialize MongoDB service.\n{ex.Message}", "Startup error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(-1);
            }
        }
    }
}