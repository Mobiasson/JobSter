using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using JobSter.Model;
using JobSter.Views;
using MongoDB.Driver;

namespace JobSter;
public partial class MainWindow : Window, INotifyPropertyChanged {
    private string _username = "Guest";

    public ObservableCollection<JobApplication> AppliedJobs { get; set; }
    public JobApplication? SelectedJob { get; set; }

    public string Username {
        get => _username;
        set {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public MainWindow() {
        InitializeComponent();
        AppliedJobs = new ObservableCollection<JobApplication>();
        if(App.CurrentUser != null) {
            Username = App.CurrentUser.Username ?? "Guest";
        }

        DataContext = this;
        LoadJobsFromDatabase();
    }

    private void LoadJobsFromDatabase() {
        if(App.MongoDb == null || App.CurrentUser == null) return;
        var filter = Builders<JobApplication>.Filter.Eq(j => j.UserId, App.CurrentUser.Id);
        var jobs = App.MongoDb.JobApplications.Find(filter).ToList();

        foreach(var job in jobs) {
            AppliedJobs.Add(job);
        }
    }

    private void btn_Minimize_Click(object sender, RoutedEventArgs e) {
        WindowState = WindowState.Minimized;
    }

    private void btn_Maximize_Click(object sender, RoutedEventArgs e) {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void btn_Close_Click(object sender, RoutedEventArgs e) {
        Application.Current.Shutdown();
    }

    private void btn_addJob_Click(object sender, RoutedEventArgs e) {
        var addJobWindow = new Views.AddJobView(this) {
            Owner = this
        };
        addJobWindow.ShowDialog();
    }

    private void btn_delete_Click(object sender, RoutedEventArgs e) {
        try {
            var selectedJob = JobListBox.SelectedItem as JobApplication;
            if(selectedJob != null) {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the application for {selectedJob.Title} at {selectedJob.CompanyName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if(result == MessageBoxResult.Yes) {
                    if(App.MongoDb != null && !string.IsNullOrEmpty(selectedJob.Id)) {
                        App.MongoDb.JobApplications.DeleteOne(j => j.Id == selectedJob.Id);
                    }
                    AppliedJobs.Remove(selectedJob);
                    MessageBox.Show("Job application deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } else {
                MessageBox.Show("Please select a job application to delete.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        catch(Exception ex) {
            MessageBox.Show($"An error occurred while deleting the job:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    public void AddJob(JobApplication job) {
        if(App.MongoDb != null && App.CurrentUser != null) {
            job.UserId = App.CurrentUser.Id;
            App.MongoDb.JobApplications.InsertOne(job);
            AppliedJobs.Add(job);
        } else {
            MessageBox.Show("Unable to add job. User not authenticated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btn_Logout_Click(object sender, RoutedEventArgs e) {
        this.Close();
        var loginView = new LoginView();
        loginView.ShowDialog();
    }

    private void btn_LogoutAndDelete_Click(object sender, RoutedEventArgs e) {
        var confirmation = MessageBox.Show(
            $"Are you sure you want to delete your account and all associated data?",
            "Delete Account Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if(confirmation == MessageBoxResult.Yes) {
            try {
                var currentUser = App.CurrentUser;

                if(App.MongoDb != null && currentUser != null && !string.IsNullOrEmpty(currentUser.Id)) {
                    var jobFilter = Builders<JobApplication>.Filter.Eq(j => j.UserId, currentUser.Id);
                    App.MongoDb.JobApplications.DeleteMany(jobFilter);
                    var userFilter = Builders<User>.Filter.Eq(u => u.Id, currentUser.Id);
                    App.MongoDb.Users.DeleteOne(userFilter);
                    App.CurrentUser = null;
                    MessageBox.Show(
                        "Your account and all data has been deleted",
                        "Account Deleted",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    this.Close();
                    var loginView = new LoginView();
                    loginView.ShowDialog();
                } else {
                    MessageBox.Show(
                        "Unable to delete account.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch(Exception ex) {
                MessageBox.Show(
                    $"An error occurred while deleting the account:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    private void ContextMenu_Delete_Click(object sender, RoutedEventArgs e) {
        try {
            var menuItem = sender as MenuItem;
            if(menuItem?.DataContext is JobApplication selectedJob) {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete this job for {selectedJob.Title} at {selectedJob.CompanyName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes) {
                    if(App.MongoDb != null && !string.IsNullOrEmpty(selectedJob.Id)) {
                        App.MongoDb.JobApplications.DeleteOne(j => j.Id == selectedJob.Id);
                    }
                    AppliedJobs.Remove(selectedJob);
                    MessageBox.Show("Job application deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        catch(Exception ex) {
            MessageBox.Show($"An error occurred while deleting the job:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void UpdateJobStatus(JobApplication job, string newStatus) {
        try {
            job.Status = newStatus;

            if(App.MongoDb != null && !string.IsNullOrEmpty(job.Id)) {
                var filter = Builders<JobApplication>.Filter.Eq(j => j.Id, job.Id);
                var update = Builders<JobApplication>.Update.Set(j => j.Status, newStatus);
                App.MongoDb.JobApplications.UpdateOne(filter, update);
            }
        }
        catch(Exception ex) {
            MessageBox.Show($"Error updating status:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ContextMenu_Denied_Click(object sender, RoutedEventArgs e) {
        if((sender as MenuItem)?.DataContext is JobApplication job)
            UpdateJobStatus(job, "Denied");
    }

    private void ContextMenu_Approval_Click(object sender, RoutedEventArgs e) {
        if((sender as MenuItem)?.DataContext is JobApplication job)
            UpdateJobStatus(job, "Approved");
    }

}