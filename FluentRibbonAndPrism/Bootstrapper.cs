using System.Windows;
using Fluent;
using FluentRibbonAndPrism.Infrastructure;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;

namespace FluentRibbonAndPrism
{
	public class Bootstrapper : WindsorBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			var mainWindow = new MainWindow
			{
				DataContext = Container.Resolve<MainWindowViewModel>()
			};
			mainWindow.Show();
			return mainWindow;
		}

		protected override IModuleCatalog GetModuleCatalog()
		{
			return new ModuleCatalog();
		}

		protected override void BeforeShellCreated()
		{
			base.BeforeShellCreated();

			Container.AddComponent<MainWindowViewModel>();
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var mappings = base.ConfigureRegionAdapterMappings();
			Container.AddComponent<RibbonRegionAdapter>();
			mappings.RegisterMapping(typeof(Ribbon), Container.Resolve<RibbonRegionAdapter>());
			return mappings;
		}
	}
}