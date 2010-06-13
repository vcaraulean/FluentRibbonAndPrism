using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace FluentRibbonAndPrism
{
	public class MainWindowViewModel
	{
		public MainWindowViewModel()
		{
			CreateNewTabCommand = new DelegateCommand<object>(CreateNewTab);
		}

		private static void CreateNewTab(object obj)
		{
			MessageBox.Show("Hello from Command");
		}

		public ICommand CreateNewTabCommand { get; set; }
	}
}