using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo
{
    using System.Reactive.Concurrency;
    using CommonServiceLocator;
    using Prism.Events;
    using Prism.Mvvm;
    using Reactive.Bindings;
    using ReactiveDemo.Constants.Enum;
    using ReactiveDemo.Schema;
    using Unity;
    using Unity.ServiceLocation;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer Container { get; } = new UnityContainer();

        private EventAggregator EventAggregator { get; } = new EventAggregator();

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);

            Container.RegisterInstance(typeof(IUnityContainer), Container);
            Container.RegisterInstance(typeof(IEventAggregator), EventAggregator);

            var provider = new UnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => provider);
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));

            // change system theme
            var schema = ThemeSchemaProvider.GetThemeSchema();
            schema.ChangeSystemTheme(ConfigEnum.SystemTheme.Normal.GetCode());

            var window = Container.Resolve<ReactiveDemoView>();
            window.ShowDialog();
        }
    }
}
