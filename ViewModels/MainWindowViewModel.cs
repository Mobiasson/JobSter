using System.ComponentModel;

namespace JobSter.ViewModels;

public class MainViewModel : INotifyPropertyChanged {
    private string _username = "Guest";

    public string Username {
        get => _username;
        set {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public void SetUser(string name) {
        Username = name;
    }
}