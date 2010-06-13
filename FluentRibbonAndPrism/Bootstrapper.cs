using System;
using System.Windows;
using FluentRibbonAndPrism.Infrastructure;
using Microsoft.Practices.Composite.Modularity;

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
	}
}