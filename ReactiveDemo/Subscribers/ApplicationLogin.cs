using ReactiveDemo.Events;
using ReactiveDemo.Views;
using ReactiveDemo.Views.Login;
using System;
using Unity;

namespace ReactiveDemo.Subscribers
{
    public class ApplicationLogin : AbstractEventSubscriber
    {
        public override void Register(LauncherContext context)
        {
            base.Register(context);

            // Login
            EventAggregator.GetEvent<LoginEvents.UserLoginRequestEvent>().Subscribe(o => ShowLogin(o));
        }

        private void ShowLogin(int args)
        {
            var window = Container.Resolve<LoginView>();
            //var window = Container.Resolve<MainWindowView>();
            window.ShowDialog();
        }
    }
}
