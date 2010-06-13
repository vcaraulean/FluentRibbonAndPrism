using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace FluentRibbonAndPrism.Infrastructure
{
	/// <summary>
	/// Defines an adapter for the <see cref="IServiceLocator"/> interface
	/// to be used by the Composite Application Library.
	/// </summary>
	public class WindsorServiceLocator : ServiceLocatorImplBase
	{
		private readonly IWindsorContainer container;

		public WindsorServiceLocator(IWindsorContainer container)
		{
			this.container = container;
		}

		protected override object DoGetInstance(Type serviceType, string key)
		{
			if (key != null)
			{
				if (!container.Kernel.HasComponent(key))
					container.AddComponentLifeStyle(key, serviceType, LifestyleType.Transient);

				return container.Resolve(key, serviceType);
			}
			if (!container.Kernel.HasComponent(serviceType))
				container.Register(Component.For(serviceType).LifeStyle.Transient);

			return container.Resolve(serviceType);
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return (object[])container.ResolveAll(serviceType);
		}
	}
}