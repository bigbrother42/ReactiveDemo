using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings.Extensions;
using Unity;

namespace ReactiveDemo.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        public CompositeDisposable DisposablePool { get; }

        public IEventAggregator EventAggregator { get; }

        public IUnityContainer Container { set; get; }

        protected Dictionary<EventBase, HashSet<SubscriptionToken>> _subscribedEvents;

        public DelegateCommand FinishInteractionCommand { get; set; }
        public Action FinishInteraction { get; set; }

        private INotification _notification;
        [JsonIgnore]
        public INotification Notification
        {
            get => _notification;
            set => SetProperty(ref _notification, value);
        }

        public ViewModelBase()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                Container = ServiceLocator.Current.GetInstance<IUnityContainer>();
                EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            }

            FinishInteractionCommand = new DelegateCommand(() =>
            {
                FinishInteraction?.Invoke();
            });

            DisposablePool = new CompositeDisposable();

            RegisterProperties();

            RegisterCommands();

            if (ServiceLocator.IsLocationProviderSet)
            {
                RegisterPubEvents();
            }

            InitData();
        }

        ~ViewModelBase()
        {
            
        }

        public void Dispose()
        {
            DisposablePool.Dispose();
        }

        public void SubscribeEvent<T>(Action act) where T : PubSubEvent, new()
        {
            PubSubEvent eve = EventAggregator.GetEvent<T>();

            var token = eve.Subscribe(act).AddTo(DisposablePool);

            if (_subscribedEvents == null) _subscribedEvents = new Dictionary<EventBase, HashSet<SubscriptionToken>>();
            if (!_subscribedEvents.ContainsKey(eve)) _subscribedEvents.Add(eve, new HashSet<SubscriptionToken>());

            _subscribedEvents[eve].Add(token);
        }

        public void SubscribeEvent<T, U>(Action<U> act) where T : PubSubEvent<U>, new()
        {

            PubSubEvent<U> eve = EventAggregator.GetEvent<T>();

            var token = eve.Subscribe(act).AddTo(DisposablePool);
            if (_subscribedEvents == null) _subscribedEvents = new Dictionary<EventBase, HashSet<SubscriptionToken>>();
            if (!_subscribedEvents.ContainsKey(eve)) _subscribedEvents.Add(eve, new HashSet<SubscriptionToken>());

            _subscribedEvents[eve].Add(token);
        }

        protected virtual void RegisterProperties()
        {
        }

        protected virtual void RegisterCommands()
        {

        }

        protected virtual void InitData()
        {

        }

        protected virtual void RegisterPubEvents()
        {

        }
    }
}