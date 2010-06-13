using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Regions;

namespace FluentRibbonAndPrism
{
	public class MainWindowViewModel
	{
		private readonly IRegionManager regionManager;

		public MainWindowViewModel(IRegionManager regionManager)
		{
			this.regionManager = regionManager;
			CreateNewTabCommand = new DelegateCommand<object>(CreateNewTab);
		}

		private void CreateNewTab(object obj)
		{
			regionManager.AddToRegion("theRibbon", new FirstRibbonTab());
		}

		public ICommand CreateNewTabCommand { get; set; }
	}
}