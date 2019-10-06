using System.Windows;
using Autofac;
using DeveloperTest.Services;
using DeveloperTest.Views;

namespace DeveloperTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitDI();

            var model = _container.Resolve<MainWindowViewModel>();
            var window = new MainWindow { DataContext = model };
            window.Show();
        }

        private void InitDI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacServicesModule>();

            //view models
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            _container = builder.Build();
        }
    }
}
