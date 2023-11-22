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
    using System.Windows.Threading;
    using CommonServiceLocator;
    using Prism.Events;
    using Prism.Mvvm;
    using Reactive.Bindings;
    using ReactiveDemo.Configure;
    using ReactiveDemo.Constants.Enum;
    using ReactiveDemo.Filters;
    using ReactiveDemo.Helpers;
    using ReactiveDemo.Schema;
    using Unity;
    using Unity.ServiceLocation;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationLauncher _launcher = new ApplicationLauncher();

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledExceptionOccured;

            _launcher.InitProcess(e);

            _launcher.Execute();

            _launcher.PostProcess();
        }

        private void OnDispatcherUnhandledExceptionOccured(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionProcessorHelper.Handle(e.Exception);
        }
    }
}
