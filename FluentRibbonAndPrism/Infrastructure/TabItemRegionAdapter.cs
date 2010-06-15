using System;
using System.Collections.Specialized;
using Fluent;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;

namespace FluentRibbonAndPrism.Infrastructure
{
	public class TabItemRegionAdapter : RegionAdapterBase<RibbonTabItem>
	{
		public TabItemRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, RibbonTabItem regionTarget)
		{
			region.Views.CollectionChanged += (sender, args) => ViewsOnCollectionChanged(regionTarget, args);
		}

		private static void ViewsOnCollectionChanged(RibbonTabItem regionTarget, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var newItem in e.NewItems)
					{
						regionTarget.Groups.Add((RibbonGroupBox) newItem);
					}
					regionTarget.BringIntoView();
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (var oldItem in e.OldItems)
					{
						regionTarget.Groups.Remove((RibbonGroupBox) oldItem);
					}
					break;
			}
		}

		protected override IRegion CreateRegion()
		{
			return new AllActiveRegion();
		}
	}
}