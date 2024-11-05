using MinimarketApp.Utilities;
using MinimarketApp.ViewModel;
using System.Diagnostics;
using System.Windows;

namespace MinimarketApp.Services
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }

    public class NavigationService : ViewModelBase, INavigationService
    {
        private readonly Func<Type, ViewModelBase> _viewModelFactory;

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase vm = _viewModelFactory.Invoke(typeof(TViewModel));
            // Debug.WriteLine("Navigate to " + vm);
            CurrentView = vm;
        }
    }
}
