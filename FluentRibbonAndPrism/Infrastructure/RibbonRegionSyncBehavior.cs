using System;
using System.Collections.Specialized;
using System.Windows;
using Fluent;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;

namespace FluentRibbonAndPrism.Infrastructure
{
	public class RibbonRegionSyncBehavior : RegionBehavior, IHostAwareRegionBehavior
	{
		private Ribbon hostControl;

		public const string BehaviorKey = "RibbonRegionSyncBehavior";

		protected override void OnAttach()
		{
			if (hostControl == null)
				throw new InvalidOperationException("Host control cannot be null.");

			SynchronizeItems();

			hostControl.SelectedTabChanged += HostControlOnSelectedTabChanged;
			Region.ActiveViews.CollectionChanged += ActiveViewsOnCollectionChanged;
			Region.Views.CollectionChanged += ViewsOnCollectionChanged;
		}

		private void ViewsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var newItem in e.NewItems)
				{
					hostControl.Tabs.Add((RibbonTabItem) newItem);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (var oldItem in e.OldItems)
				{
					hostControl.Tabs.Remove((RibbonTabItem) oldItem);
				}
			}
		}

		private void ActiveViewsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
				hostControl.SelectedTabItem = (RibbonTabItem) notifyCollectionChangedEventArgs.NewItems[0];
			
			else if(notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove
				&& hostControl.SelectedTabItem != null
				&& notifyCollectionChangedEventArgs.OldItems.Contains(hostControl.SelectedTabItem))
			{
				hostControl.SelectedTabItem = null;
			}
		}

		private void HostControlOnSelectedTabChanged(object sender, EventArgs eventArgs)
		{
			if (hostControl.SelectedTabItem == null)
				return;

			Region.Activate(hostControl.SelectedTabItem);
		}

		private void SynchronizeItems()
		{
			// TODO: initial item synchronization: copy from region to ribbon and vice-versa
		}

		public DependencyObject HostControl
		{
			get { return hostControl; }
			set
			{
				var ribbon = value as Ribbon;
				if (ribbon == null)
					throw new InvalidOperationException("Host control is to be set to null!");

				hostControl = ribbon;
			}
		}
	}
}