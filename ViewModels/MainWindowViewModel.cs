using System.ComponentModel;

namespace JobSter.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _username = "Guest";  // default

    public string Username {
        get => _username;
        set {
            _username = value;
            OnPropertyChanged(nameof(Username));  // notify UI
        }
    }

    // INotifyPropertyChanged implementation (simple version)
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Example: call this when user logs in
    public void SetUser(string name) {
        Username = name;  // e.g. "Mikael"
    }
}
