using System;
using System.Linq;
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
			CreateAndActivateNewTabCommand = new DelegateCommand<object>(CreateAndActivateNewTab);
			RemoveActiveTabCommand = new DelegateCommand<object>(RemoveActiveTab, CanRemoveActiveTab);
			DeactivateActiveTabCommand = new DelegateCommand<object>(DeactivateActiveTab);
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

		private void CreateAndActivateNewTab(object obj)
		{
			var ribbonTab = CreateNewTab();
			RibbonRegion.Add(ribbonTab);
			RibbonRegion.Activate(ribbonTab);

			RemoveActiveTabCommand.RaiseCanExecuteChanged();
		}

		private void DeactivateActiveTab(object obj)
		{
			var activeTab = RibbonRegion.ActiveViews.FirstOrDefault();
			if (activeTab != null)
				RibbonRegion.Deactivate(activeTab);
		}

		private bool CanRemoveActiveTab(object arg)
		{
			if (RibbonRegion == null)
				return false;
			return RibbonRegion.ActiveViews.Count() > 0;
		}

		private void RemoveActiveTab(object obj)
		{
			var activeView = RibbonRegion.ActiveViews.First();
			RibbonRegion.Remove(activeView);

			RemoveActiveTabCommand.RaiseCanExecuteChanged();
		}

		private void CreateNewTab(object obj)
		{
			RibbonRegion.Add(CreateNewTab());

			RemoveActiveTabCommand.RaiseCanExecuteChanged();
		}

		private NewRibbonTab CreateNewTab()
		{
			var newTab = new NewRibbonTab();
			var tabCount = RibbonRegion.Views.Count() + 1;
			newTab.Header += "Nr. " + tabCount;
			return newTab;
		}

		public DelegateCommand<object> CreateNewTabCommand { get; set; }
		public DelegateCommand<object> CreateAndActivateNewTabCommand { get; set; }
		public DelegateCommand<object> RemoveActiveTabCommand { get; set; }
		public DelegateCommand<object> DeactivateActiveTabCommand { get; set; }
	}
}