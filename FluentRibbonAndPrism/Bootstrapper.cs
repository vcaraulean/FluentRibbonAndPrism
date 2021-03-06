using System.Windows;
using Castle.MicroKernel.Registration;
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
			var mainWindow = new MainWindow();

			mainWindow.Show();

			mainWindow.DataContext = Container.Resolve<MainWindowViewModel>();
			
			return mainWindow;
		}

		protected override void AfterShellCreated()
		{
			base.AfterShellCreated();
			var mainViewModel = Container.Resolve<MainWindowViewModel>();
			mainViewModel.AfterShellCreated();
		}

		protected override IModuleCatalog GetModuleCatalog()
		{
			return new DirectoryModuleCatalog {ModulePath = @"."};
		}

		protected override void BeforeShellCreated()
		{
			base.BeforeShellCreated();

			Container.Register(Component.For<MainWindowViewModel>());
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var mappings = base.ConfigureRegionAdapterMappings();
			Container.Register(Component.For<RibbonRegionAdapter>());
			Container.Register(Component.For<TabItemRegionAdapter>());

			mappings.RegisterMapping(typeof(Ribbon), Container.Resolve<RibbonRegionAdapter>());
			mappings.RegisterMapping(typeof(RibbonTabItem), Container.Resolve<TabItemRegionAdapter>());
			return mappings;
		}
	}
}