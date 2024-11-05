using MinimarketApp.Repository;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace MinimarketApp.Utilities
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected float ParseFloat(string value)
        {
            float result = 0;
            float.TryParse(value, out result);
            return result;
        }

        protected int ParseInt(string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        private string _currentDate;
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }

        private string _currentDateTime;
        public string CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
                CurrentDate = DateTime.UtcNow.ToString(@"dd MMMM yyyy", new CultureInfo("id-ID"));
            }
        }

        private string _currentUsername;
        public string CurrentUsername
        {
            get { return _currentUsername; }
            set
            {
                _currentUsername = value;
                OnPropertyChanged();
            }
        }

        private UserRepository _userRepository = new();

        public ViewModelBase()
        {
            CurrentDateTime = DateTime.Now.ToString(@"dd MMMM yyyy, HH\:mm\:ss", new CultureInfo("id-ID"));
            // CurrentDate = DateTime.Now.ToString(@"dd MMMM yyyy", new CultureInfo("id-ID"));
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(0.1);
            LiveTime.Tick += TimerTick;
            LiveTime.Start();

            CurrentUsername = _userRepository.GetCurrentUserData().username;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            // CurrentDate = DateTime.UtcNow.ToString(@"dd MMMM yyyy", new CultureInfo("id-ID"));
            // CurrentDate = DateTime.Now.ToString(@"dd MMMM yyyy", new CultureInfo("id-ID"));
            CurrentDateTime = DateTime.Now.ToString(@"dd MMMM yyyy, HH\:mm\:ss", new CultureInfo("id-ID"));
        }

        protected string FormattedCurrency(float amount)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            return amount.ToString("C", culture);
        }

        protected string FormattedDate(DateTime dateTime)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            string formattedDate = dateTime.ToString("dd MMMM yyyy", culture);
            return formattedDate;
        }

        protected string FormattedSqlDate(DateTime dateTime)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            string formattedDateTime = dateTime.ToString("yyyy-MM-dd", culture);
            return formattedDateTime;
        }
    }
}
