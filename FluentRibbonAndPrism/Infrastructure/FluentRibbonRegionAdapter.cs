using System;
using System.Collections.Specialized;
using System.Linq;
using Fluent;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;

namespace FluentRibbonAndPrism.Infrastructure
{
	public class FluentRibbonRegionAdapter : RegionAdapterBase<Ribbon>
	{
		public FluentRibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, Ribbon regionTarget)
		{
			region.Views.CollectionChanged += (sender, notifyCollectionChangedEventArgs) => 
				HandleChangesInViewCollection(notifyCollectionChangedEventArgs, regionTarget);

			foreach (RibbonTabItem view in region.Views)
			{
				regionTarget.Tabs.Add(view);
			}
		}

		private static void HandleChangesInViewCollection(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs, Ribbon regionTarget)
		{
			switch (notifyCollectionChangedEventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
					{
						regionTarget.Tabs.Add((RibbonTabItem) newItem);
					}
					regionTarget.SelectedTabItem = (RibbonTabItem) notifyCollectionChangedEventArgs.NewItems[0];
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
					{
						regionTarget.Tabs.Remove((RibbonTabItem) oldItem);
					}
					break;
			}
		}

		protected override IRegion CreateRegion()
		{
			return new FluentRibbonRegion();
		}
	}

	public class FluentRibbonRegion : SingleActiveRegion
	{
	}
}