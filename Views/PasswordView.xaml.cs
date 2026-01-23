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
        Close();
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}
