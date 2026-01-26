using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using JobSter.Model;
using MongoDB.Driver;

namespace JobSter;
public partial class MainWindow : Window {
    public ObservableCollection<JobApplication> AppliedJobs { get; set; }
    public JobApplication? SelectedJob { get; set; }

    public MainWindow() {
        InitializeComponent();
        AppliedJobs = new ObservableCollection<JobApplication>();
        DataContext = this;
        LoadJobsFromDatabase();
    }

    private void LoadJobsFromDatabase() {
        if(App.MongoDb == null) return;

        var jobs = App.MongoDb.JobApplications.Find(_ => true).ToList();
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
                    $"Are you sure you want to delete {selectedJob.Title} at {selectedJob.CompanyName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes) {
                    if(App.MongoDb != null && !string.IsNullOrEmpty(selectedJob.Id)) {
                        App.MongoDb.JobApplications.DeleteOne(j => j.Id == selectedJob.Id);
                    }
                    AppliedJobs.Remove(selectedJob);
                    MessageBox.Show("Job deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
        if(App.MongoDb != null) {
            App.MongoDb.JobApplications.InsertOne(job);
        }

        AppliedJobs.Add(job);
    }
}