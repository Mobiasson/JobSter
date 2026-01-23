using System.Windows;
using JobSter.ViewModels;

namespace JobSter.Views;
public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        var passwordView = new PasswordView();
        passwordView.Show();

    }

    private void btn_Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}