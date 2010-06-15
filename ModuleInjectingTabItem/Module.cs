using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;

namespace ModuleInjectingTabItem
{
	[Module(ModuleName = "ModuleInjectingTabItem", OnDemand = true)]
	public class Module : IModule
	{
		private readonly IRegionManager regionManager;

		public Module(IRegionManager regionManager)
		{
			this.regionManager = regionManager;
		}

		public void Initialize()
		{
			var tab = new RibbonTabFromModule();

			var ribbonRegion = regionManager.Regions["theRibbon"];

			ribbonRegion.Add(tab);
			ribbonRegion.Activate(tab);
		}
	}
}
