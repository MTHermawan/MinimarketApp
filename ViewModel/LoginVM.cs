using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        private UserRepository _userRepository = new();
        public LoginVM()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        private string? inputUsername;
        public string? InputUsername
        {
            get { return inputUsername; }
            set
            {
                inputUsername = value;
                OnPropertyChanged();
            }
        }

        private string? inputPassword;
        public string? InputPassword
        {
            get { return inputPassword; }
            set
            {
                inputPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private bool CanLogin(object obj)
        {
            return !string.IsNullOrEmpty(InputUsername) && !string.IsNullOrEmpty(InputPassword);
        }

        private void Login(object obj)
        {
            string? level = _userRepository.Login(InputUsername, InputPassword);
            Session.Username = InputUsername;
            Session.Level = level;
            switch (level)
            {
                case "1":
                case "2":
                    ((App)Application.Current).OpenMainWindow();
                    break;
                default:
                    ErrorMessage = "Username atau Password salah!";
                    break;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                {
                    ErrorMessageActive = !string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private Visibility _errorMessageActive = Visibility.Collapsed;
        public Visibility ErrorMessageActive{
            get { return _errorMessageActive; }
            set
            {
                _errorMessageActive = value;
                OnPropertyChanged();
            }
        }
    }
}
