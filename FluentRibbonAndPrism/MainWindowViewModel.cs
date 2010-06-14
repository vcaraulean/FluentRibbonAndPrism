using System;
using System.Linq;
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
			CreateAndActivateNewTabCommand = new DelegateCommand<object>(CreateAndActivateNewTab);
			RemoveActiveTabCommand = new DelegateCommand<object>(RemoveActiveTab, CanRemoveActiveTab);
			DeactivateActiveTabCommand = new DelegateCommand<object>(DeactivateActiveTab, CanDeactivateActiveTab);
		}

		private IRegion RibbonRegion
		{
			get
			{
				if (regionManager.Regions.ContainsRegionWithName("theRibbon"))
					return regionManager.Regions["theRibbon"];

				return null;
			}
		}

		private void DeactivateActiveTab(object obj)
		{
			var activeTab = RibbonRegion.ActiveViews.First();
			RibbonRegion.Deactivate(activeTab);
		}

		private bool CanDeactivateActiveTab(object arg)
		{
			return RegionHasActiveViews();
		}

		private void RemoveActiveTab(object obj)
		{
			var activeView = RibbonRegion.ActiveViews.First();
			RibbonRegion.Remove(activeView);
		}

		private bool CanRemoveActiveTab(object arg)
		{
			return RegionHasActiveViews();
		}

		private bool RegionHasActiveViews()
		{
			if (RibbonRegion == null)
				return false;
			return RibbonRegion.ActiveViews.Count() > 0;
		}

		private void CreateNewTab(object obj)
		{
			RibbonRegion.Add(CreateNewTab());
		}

		private NewRibbonTab CreateNewTab()
		{
			var newTab = new NewRibbonTab();
			var tabCount = RibbonRegion.Views.Count() + 1;
			newTab.Header += "Nr. " + tabCount;
			return newTab;
		}

		private void CreateAndActivateNewTab(object obj)
		{
			var ribbonTab = CreateNewTab();
			RibbonRegion.Add(ribbonTab);
			RibbonRegion.Activate(ribbonTab);
		}

		public DelegateCommand<object> CreateNewTabCommand { get; set; }
		public DelegateCommand<object> CreateAndActivateNewTabCommand { get; set; }
		public DelegateCommand<object> RemoveActiveTabCommand { get; set; }
		public DelegateCommand<object> DeactivateActiveTabCommand { get; set; }

		public void AfterShellCreated()
		{
			RibbonRegion.ActiveViews.CollectionChanged += delegate
			{
				RemoveActiveTabCommand.RaiseCanExecuteChanged();
				DeactivateActiveTabCommand.RaiseCanExecuteChanged();
			};
		}
	}
}