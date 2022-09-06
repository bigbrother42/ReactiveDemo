namespace ReactiveDemo.ActionBase
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interactivity;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using ViewModels;

    public class AlertWindowAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
            "Owner", typeof(DependencyObject), typeof(AlertWindowAction), new PropertyMetadata(null));

        public DependencyObject Owner
        {
            get => (DependencyObject)GetValue(OwnerProperty);
            set => SetValue(OwnerProperty, value);
        }

        public static readonly DependencyProperty WindowNameProperty = DependencyProperty.Register(
            "WindowName", typeof(string), typeof(AlertWindowAction), new PropertyMetadata(string.Empty));

        public string WindowName
        {
            get => (string)GetValue(WindowNameProperty);
            set => SetValue(WindowNameProperty, value);
        }

        protected override void Invoke(object parameter)
        {
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
                    Action callback = null;
                    if (parameter is InteractionRequestedEventArgs args)
                    {
                        callback = args.Callback;
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
                        viewModelBase.FinishInteraction = () =>
                        {
                            win.Close();
                        };

                        win.Closing += (sender, eventArgs) =>
                        {
                            viewModelBase.Dispose();
                        };
                    }

                    win.WindowState = WindowState.Normal;
                    win.Activate();
                    win.ShowDialog();
                }
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