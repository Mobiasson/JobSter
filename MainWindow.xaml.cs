using System.Windows;
using JobSter.ViewModels;
using JobSter.Views;

namespace JobSter;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
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

    private void btn_delete_Click(object sender, RoutedEventArgs e) {

    }

    private void btn_addJob_Click(object sender, RoutedEventArgs e) {
        var addJobWindow = new AddJobView();
        addJobWindow.Owner = this;
        addJobWindow.ShowDialog();
    }
}