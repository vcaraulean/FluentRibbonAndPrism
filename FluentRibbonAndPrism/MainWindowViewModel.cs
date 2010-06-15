using System;
using System.Linq;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Regions;

namespace FluentRibbonAndPrism
{
	public class MainWindowViewModel
	{
		private readonly IRegionManager regionManager;
		private readonly IModuleManager moduleManager;
		private IRegion ribbonRegion;

		public MainWindowViewModel(IRegionManager regionManager, IModuleManager moduleManager) 
		{
			this.regionManager = regionManager;
			this.moduleManager = moduleManager;

			CreateNewTabCommand = new DelegateCommand<object>(CreateNewTab);
			CreateAndActivateNewTabCommand = new DelegateCommand<object>(CreateAndActivateNewTab);
			RemoveActiveTabCommand = new DelegateCommand<object>(RemoveActiveTab, CanRemoveActiveTab);
			DeactivateActiveTabCommand = new DelegateCommand<object>(DeactivateActiveTab, CanDeactivateActiveTab);
			LoadModuleAndShowTabCommand = new DelegateCommand<object>(LoadModule);
			DemoTabAsRegionCommand = new DelegateCommand<object>(DemoTabAsRegion);
		}

		private void DemoTabAsRegion(object obj)
		{
			var tabItem = new RibbonTabAsRegion();
			ribbonRegion.Add(tabItem);
			ribbonRegion.Activate(tabItem);
		}

		private void LoadModule(object obj)
		{
			moduleManager.LoadModule("ModuleInjectingTabItem");
		}

		private void DeactivateActiveTab(object obj)
		{
			var activeTab = ribbonRegion.ActiveViews.First();
			ribbonRegion.Deactivate(activeTab);
		}

		private bool CanDeactivateActiveTab(object arg)
		{
			return RegionHasActiveViews();
		}

		private void RemoveActiveTab(object obj)
		{
			var activeView = ribbonRegion.ActiveViews.First();
			ribbonRegion.Remove(activeView);
		}

		private bool CanRemoveActiveTab(object arg)
		{
			return RegionHasActiveViews();
		}

		private bool RegionHasActiveViews()
		{
			if (ribbonRegion == null)
				return false;
			return ribbonRegion.ActiveViews.Count() > 0;
		}

		private void CreateNewTab(object obj)
		{
			ribbonRegion.Add(CreateNewTab());
		}

		private NewRibbonTab CreateNewTab()
		{
			var newTab = new NewRibbonTab();
			var tabCount = ribbonRegion.Views.Count() + 1;
			newTab.Header += "Nr. " + tabCount;
			return newTab;
		}

		private void CreateAndActivateNewTab(object obj)
		{
			var ribbonTab = CreateNewTab();
			ribbonRegion.Add(ribbonTab);
			ribbonRegion.Activate(ribbonTab);
		}

		public DelegateCommand<object> CreateNewTabCommand { get; set; }
		public DelegateCommand<object> CreateAndActivateNewTabCommand { get; set; }
		public DelegateCommand<object> RemoveActiveTabCommand { get; set; }
		public DelegateCommand<object> DeactivateActiveTabCommand { get; set; }
		public DelegateCommand<object> LoadModuleAndShowTabCommand { get; set; }
		public DelegateCommand<object> DemoTabAsRegionCommand { get; set; }

		public void AfterShellCreated()
		{
			ribbonRegion = regionManager.Regions["theRibbon"];

			ribbonRegion.ActiveViews.CollectionChanged += delegate
			{
				RemoveActiveTabCommand.RaiseCanExecuteChanged();
				DeactivateActiveTabCommand.RaiseCanExecuteChanged();
			};
		}
	}
}