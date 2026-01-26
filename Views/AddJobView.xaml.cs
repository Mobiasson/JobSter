using System.Windows;

namespace JobSter.Views;
public partial class AddJobView : Window {
    public AddJobView() {
        InitializeComponent();
    }
    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Close();
    }
}
