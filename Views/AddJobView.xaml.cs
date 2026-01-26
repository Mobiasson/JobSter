using System;
using System.Windows;
using JobSter.Model;

namespace JobSter.Views;

public partial class AddJobView : Window {
    private readonly MainWindow _mainWindow;

    public AddJobView(MainWindow mainWindow) {
        InitializeComponent();
        _mainWindow = mainWindow;
        dp_DateApplied.SelectedDate = DateTime.Today;
    }

    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    private void btn_AddJob_Click(object sender, RoutedEventArgs e) {
        if(string.IsNullOrWhiteSpace(txt_CompanyName.Text)) {
            MessageBox.Show("Must fill field.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if(string.IsNullOrWhiteSpace(txt_Position.Text)) {
            MessageBox.Show("Must fill field.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if(!dp_DateApplied.SelectedDate.HasValue) {
            MessageBox.Show("Must fill field", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newJob = new JobApplication {
            CompanyName = txt_CompanyName.Text.Trim(),
            Title = txt_Position.Text.Trim(),
            AppliedAt = dp_DateApplied.SelectedDate.Value,
            Status = "Pending"
        };

        _mainWindow.AddJob(newJob);
        Close();
    }
}
