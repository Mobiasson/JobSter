using System.ComponentModel;
using System.Text.RegularExpressions;

namespace JobSter.ViewModels;
public class LoginViewModel : INotifyPropertyChanged {
    private string _username = string.Empty;
    private string _usernameError = string.Empty;
    public bool IsValid => string.IsNullOrEmpty(UsernameError);

    public string Username {
        get => _username;
        set {
            if(_username == value) return;
            _username = value;
            OnPropertyChanged(nameof(Username));
            UsernameValidation();
        }
    }

    public string UsernameError {
        get => _usernameError;
        private set {
            if(_usernameError == value) return;
            _usernameError = value;
            OnPropertyChanged(nameof(UsernameError));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    private void UsernameValidation() {
        if(string.IsNullOrWhiteSpace(Username))
            UsernameError = "Username is required";
        else if(Username.Length < 3 || Username.Length > 20)
            UsernameError = "Username must be 3–20 characters.";
        else if(!Regex.IsMatch(Username, "^[a-zA-Z0-9]+$"))
            UsernameError = "Only letters and digits are allowed.";
        else
            UsernameError = string.Empty;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}