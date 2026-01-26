using System.Windows;
using JobSter.ViewModels;

namespace JobSter.Views;
public partial class LoginView : Window {
    public LoginView() {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e) {
        var vm = DataContext as LoginViewModel;
        var username = vm?.Username ?? string.Empty;
        if(string.IsNullOrWhiteSpace(username)) {
            MessageBox.Show("Must enter a username", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var passwordView = new PasswordView(username) { Owner = this };
        var result = passwordView.ShowDialog();
        if(result == true) {
            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }
    }

    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Application.Current.Shutdown();
    }
}