using System.Windows;
using JobSter.ViewModels;

namespace JobSter.Views; 
public partial class LoginView : Window {
    public LoginView() {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }
}