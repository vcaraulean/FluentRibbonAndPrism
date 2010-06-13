using System;
using System.Collections.Specialized;
using System.Linq;
using Fluent;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;

namespace FluentRibbonAndPrism.Infrastructure
{
	public class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
	{
		public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, Ribbon regionTarget)
		{
		}

		protected override void AttachBehaviors(IRegion region, Ribbon regionTarget)
		{
			base.AttachBehaviors(region, regionTarget);

			if (!region.Behaviors.ContainsKey(RibbonRegionSyncBehavior.BehaviorKey))
			{
				var regionBehavior = new RibbonRegionSyncBehavior {HostControl = regionTarget};
				region.Behaviors.Add(RibbonRegionSyncBehavior.BehaviorKey, regionBehavior);
			}
		}

		protected override IRegion CreateRegion()
		{
			return new SingleActiveRegion();
		}
	}
}