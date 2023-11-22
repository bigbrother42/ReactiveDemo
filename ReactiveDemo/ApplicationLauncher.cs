using Prism.Events;
using ReactiveDemo.Constants.Enum;
using ReactiveDemo.Filters;
using ReactiveDemo.Schema;
using ReactiveDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace ReactiveDemo
{
    public class ApplicationLauncher
    {
        public IFilter Filter { get; private set; }

        public LauncherContext Context { get; private set; }

        public ApplicationLauncher()
        {
            Filter = new BaseFilter();

            Context = new LauncherContext()
            {
                Container = new UnityContainer(),
                EventAggregator = new EventAggregator()
            };
        }

        public void InitProcess(StartupEventArgs e = null)
        {
            Filter.SetNext(new InitializeContainerFilter());
        }

        public void Execute()
        {
            Filter.Handle(Context);
        }

        public void PostProcess()
        {
            // change system theme
            var schema = ThemeSchemaProvider.GetThemeSchema();
            schema.ChangeSystemTheme(ConfigEnum.SystemTheme.Normal.GetCode());

            var window = Context.Container.Resolve<ReactiveDemoView>();
            window.ShowDialog();
        }
    }
}
