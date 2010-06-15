using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fluent;
using Microsoft.Practices.ServiceLocation;

namespace FluentRibbonAndPrism
{
	/// <summary>
	/// Interaction logic for FirstRibbonTab.xaml
	/// </summary>
	public partial class RibbonTabAsRegion : RibbonTabItem
	{
		public RibbonTabAsRegion()
		{
			InitializeComponent();
			DataContext = ServiceLocator.Current.GetInstance<RibbonTabAsRegionViewModel>();
		}
	}
}
