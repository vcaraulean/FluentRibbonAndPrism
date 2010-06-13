using System;
using System.Windows;
using FluentRibbonAndPrism.PrismIntegration;
using Microsoft.Practices.Composite.Modularity;

namespace FluentRibbonAndPrism
{
	public class Bootstrapper : WindsorBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			var mainWindow = new MainWindow();
			mainWindow.Show();
			return mainWindow;
		}

		protected override IModuleCatalog GetModuleCatalog()
		{
			return new ModuleCatalog();
		}
	}
}