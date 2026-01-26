using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JobSter.Model;
using JobSter.Views;
using MongoDB.Driver;

namespace JobSter {
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
            if(App.MongoDb == null || App.CurrentUser == null) return;

            // Filter jobs by current user
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
                        // Delete from MongoDB first
                        if(App.MongoDb != null && !string.IsNullOrEmpty(selectedJob.Id)) {
                            App.MongoDb.JobApplications.DeleteOne(j => j.Id == selectedJob.Id);
                        }

                        // Then remove from UI
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
                // Associate job with current user
                job.UserId = App.CurrentUser.Id;
                App.MongoDb.JobApplications.InsertOne(job);
                AppliedJobs.Add(job);
            } else {
                MessageBox.Show("Unable to add job. User not authenticated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}