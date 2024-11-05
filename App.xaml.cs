using Microsoft.Extensions.DependencyInjection;
using MinimarketApp.Services;
using MinimarketApp.Utilities;
using MinimarketApp.View;
using MinimarketApp.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinimarketApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        private readonly IServiceCollection services = new ServiceCollection();

        public App()
        {
            ResetServicesPrivilege();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType =>
            {
                var service = serviceProvider.GetService(viewModelType);
                if (service == null)
                {
                    return null;
                }
                return (ViewModelBase)service;
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            OpenLoginWindow();
            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.KeyUpEvent,
                new KeyEventHandler(TextBox_KeyUp));

            EventManager.RegisterClassHandler(typeof(ComboBox),
                ComboBox.KeyUpEvent,
                new KeyEventHandler(ComboBox_KeyUp));

            EventManager.RegisterClassHandler(typeof(ComboBox),
                ComboBox.PreviewKeyDownEvent,
                new KeyEventHandler(ComboBox_PreviewKeyDown));

            base.OnStartup(e);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            // your event handler here
            e.Handled = true;
            var textBox = sender as TextBox;
            textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(textBox), 0, Key.Insert)
            {
                RoutedEvent = UIElement.KeyUpEvent
            });
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            // your event handler here
            e.Handled = true;

        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            // your event handler here
            e.Handled = true;
        }

        public Window? loginWindow;
        public Window? MainWindow;

        private void OpenLoginWindow()
        {
            _serviceProvider.GetRequiredService<MainWindow>()?.Close();
            AddServicesPrivilege();
            _serviceProvider.GetRequiredService<LoginWindow>()?.Show();
        }

        public void OpenMainWindow()
        {
            _serviceProvider.GetRequiredService<LoginWindow>()?.Close();
            AddServicesPrivilege();
            if (Session.IsLoggedIn())
            {
                MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                _serviceProvider.GetRequiredService<MainWindow>()?.Show();
            }
        }


        public void AddServicesPrivilege()
        {
            ResetServicesPrivilege();

            if (Session.IsAdmin())
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<DashboardVM>();
                services.AddSingleton<ProdukVM>();
                services.AddSingleton<KategoriVM>();
                services.AddSingleton<SatuanVM>();
                services.AddSingleton<PelangganVM>();
                services.AddSingleton<UserVM>();
                services.AddSingleton<TransaksiVM>();
                services.AddSingleton<TransaksiPembelianVM>();
                services.AddSingleton<PemasokVM>();
                services.AddSingleton<LevelVM>();
                services.AddSingleton<RiwayatPembelianVM>();
                services.AddSingleton<RiwayatPenjualanVM>();
                services.AddSingleton<KaryawanVM>();
                services.AddSingleton<StokKeluarVM>();
                services.AddSingleton<DiskonVM>();
                services.AddSingleton<GrupProdukVM>();
            }
            else if (Session.IsKasir())
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<DashboardVM>();
                services.AddSingleton<ProdukVM>();
                services.AddSingleton<PelangganVM>();
                services.AddSingleton<TransaksiVM>();
                services.AddSingleton<RiwayatPenjualanVM>();
                services.AddSingleton<StokKeluarVM>();
                services.AddSingleton<DiskonVM>();
                services.AddSingleton<GrupProdukVM>();
            }

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType =>
            {
                var service = serviceProvider.GetService(viewModelType);
                if (service == null)
                    Logout();

                return (ViewModelBase)service;
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        private void ResetServicesPrivilege()
        {
            services.Clear();
            services.AddSingleton(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton(provider => new LoginWindow()
            {
                DataContext = provider.GetRequiredService<LoginVM>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginVM>();
        }

        public void Logout()
        {
            Session.Destroy();
            OpenLoginWindow();
        }


        void MoveToNextUIElement(KeyEventArgs e)
        {
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }

    }
}
