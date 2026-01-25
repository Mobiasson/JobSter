using System.Windows;
using JobSter.Model;

namespace JobSter.Views;
public partial class PasswordView : Window {
    private readonly string _username = string.Empty;

    public PasswordView() {
        InitializeComponent();
    }

    public PasswordView(string username) : this() {
        _username = username ?? string.Empty;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
        var pass = PasswordInput.Password;
        var confirm = ConfirmPassword.Password;
        var bothFilled = !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(confirm);
        var match = bothFilled && pass == confirm;
        btn_Confirm.IsEnabled = match;
        PasswordMismatchMessage.Visibility = bothFilled && !match ? Visibility.Visible : Visibility.Collapsed;
    }

    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    private void btn_Back_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    private void btn_Confirm_Click(object sender, RoutedEventArgs e) {
        var service = App.MongoDb;
        if(service is not null) {
            var user = new User {
                Username = _username,
                Password = PasswordInput.Password
            };
            service.CreateUser(user);
        }

        var mainWindow = new MainWindow();
        Application.Current.MainWindow = mainWindow;
        mainWindow.Show();
        Close();
        Owner?.Close();
        foreach(var login in Application.Current.Windows.OfType<LoginView>().ToList()) {
            login.Close();
        }
    }
}
