using System.Linq;
using System.Windows;

namespace JobSter.Views;
public partial class PasswordView : Window {
    public PasswordView() {
        InitializeComponent();
    }

    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    private void btn_Back_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    private void btn_Confirm_Click(object sender, RoutedEventArgs e) {
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
