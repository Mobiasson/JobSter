using System.Windows;
using JobSter.ViewModels;

namespace JobSter.Views;
public partial class LoginView : Window {
    public LoginView() {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e) {
        var passwordView = new PasswordView();
        passwordView.Show();

    }

    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    private void btn_Maximize_Click(object sender, RoutedEventArgs e) {
        if(this.WindowState == WindowState.Normal) {
            this.WindowState = WindowState.Maximized;
        } else {
            this.WindowState = WindowState.Normal;
        }

    }

    private void btn_Minimize_Click(object sender, RoutedEventArgs e) {
        if(this.WindowState == WindowState.Normal) {
            this.WindowState = WindowState.Minimized;
        } else if(this.WindowState == WindowState.Maximized) {
            this.WindowState = WindowState.Minimized;
        } else {
            this.WindowState = WindowState.Normal;
        }
    }
}