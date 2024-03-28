namespace ReactiveDemo.ActionBase
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using Prism.Common;
    using Prism.Interactivity.InteractionRequest;
    using ViewModels;
    using InteractionRequestedEventArgs = Prism.Interactivity.InteractionRequest.InteractionRequestedEventArgs;

    public class AlertWindowAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
            "Owner", typeof(DependencyObject), typeof(AlertWindowAction), new PropertyMetadata(null));
        public DependencyObject Owner
        {
            get => (DependencyObject)GetValue(OwnerProperty);
            set => SetValue(OwnerProperty, value);
        }

        public static readonly DependencyProperty CloseOwnerProperty = DependencyProperty.Register(
            "CloseOwner", typeof(bool), typeof(AlertWindowAction), new PropertyMetadata(false));
        public bool CloseOwner
        {
            get => (bool)GetValue(CloseOwnerProperty);
            set => SetValue(CloseOwnerProperty, value);
        }

        public static readonly DependencyProperty WindowNameProperty = DependencyProperty.Register(
            "WindowName", typeof(string), typeof(AlertWindowAction), new PropertyMetadata(string.Empty));
        public string WindowName
        {
            get => (string)GetValue(WindowNameProperty);
            set => SetValue(WindowNameProperty, value);
        }

        public static readonly DependencyProperty IsShowDialogProperty = DependencyProperty.Register(
            "IsShowDialog", typeof(bool), typeof(AlertWindowAction), new PropertyMetadata(true));
        public bool IsShowDialog
        {
            get => (bool)GetValue(IsShowDialogProperty);
            set => SetValue(IsShowDialogProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            INotification notification = null;
            if (parameter is InteractionRequestedEventArgs arg)
            {
                notification = arg.Context;
            }

            Assembly ass = GetAssemblyByClassName(WindowName);

            if (ass != null)
            {
                var windowType = ass.GetType(WindowName);
                var win = GetWindowByType(windowType);

                if (win == null)
                {
                    win = ass.CreateInstance(WindowName) as Window;
                }

                if (win != null)
                {
                    if (win.DataContext is ViewModelBase vbb)
                    {
                        vbb.Notification = notification;
                    }

                    Action<IInteractionRequestAware> action = null;
                    action = o =>
                    {
                        o.Notification = notification;

                        o.FinishInteraction = async () =>
                        {
                            await System.Windows.Threading.Dispatcher.Yield();
                        };
                    };
                    MvvmHelpers.ViewAndViewModelAction(win, action);

                    AttachOtherProperties(win, parameter);

                    win.WindowState = WindowState.Normal;
                    win.Activate();

                    if (IsShowDialog)
                    {
                        win.ShowDialog();
                    }
                    else
                    {
                        win.Show();
                    }
                }
            }
        }

        protected void AttachOtherProperties(Window win, object parameter)
        {
            Action callback = null;
            INotification notification = null;
            if (parameter is InteractionRequestedEventArgs args)
            {
                callback = args.Callback;
                notification = args.Context;
            }

            if (callback != null)
            {
                void WinOnClosed(object sender, EventArgs e)
                {
                    win.Closed -= WinOnClosed;

                    callback();
                }

                win.Closed += WinOnClosed;
            }

            if (win.DataContext is ViewModelBase viewModelBase)
            {
                viewModelBase.Notification = notification;

                viewModelBase.FinishInteraction = () =>
                {
                    win.Close();
                };

                win.Closing += (sender, eventArgs) =>
                {
                    viewModelBase.Dispose();
                };
            }

            if (Owner != null)
            {
                if (Owner is Window ow && ow.IsVisible)
                {
                    win.Owner = ow;
                    Owner = ow;
                }
                else if (Owner is UserControl ctr)
                {
                    var ow1 = Window.GetWindow(ctr);
                    win.Owner = ow1;

                    Owner = ow1;
                }
            }

            if (CloseOwner)
            {
                win.Owner = null;
                (Owner as Window)?.Close();
            }
        }

        protected Window GetWindowByType(Type windowType)
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType() == windowType)
                {
                    return win;
                }
            }

            return null;
        }

        protected Assembly GetAssemblyByClassName(string windowName)
        {
            var nameSpaceArr = windowName.Split('.');
            if (nameSpaceArr.Length < 2) return null;
            var assemblyTemp = string.Empty;

            var length = nameSpaceArr.Length - 2;
            for (var i = 0; i <= length; i++)
            {
                assemblyTemp = assemblyTemp + nameSpaceArr[i];
                if (i == length) continue;
                assemblyTemp = assemblyTemp + ".";
            }

            try
            {
                return Assembly.Load(assemblyTemp);
            }
            catch (Exception e)
            {
                return GetAssemblyByClassName(assemblyTemp);
            }
        }
    }
}