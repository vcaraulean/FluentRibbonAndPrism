using System;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Regions;

namespace FluentRibbonAndPrism
{
	public class RibbonTabAsRegionViewModel
	{
		private readonly IRegionManager regionManager;

		public RibbonTabAsRegionViewModel(IRegionManager regionManager)
		{
			this.regionManager = regionManager;

			ShowGroupBoxInRegionCommand = new DelegateCommand<object>(ShowGroupBoxInRegion);
		}

		private void ShowGroupBoxInRegion(object obj)
		{
			regionManager.AddToRegion("TabRegion", new GroupBox());
		}

		public DelegateCommand<object> ShowGroupBoxInRegionCommand { get; set; }
	}
}