using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace FluentRibbonAndPrism.PrismIntegration
{
	/// <summary>
	/// Defines an adapter for the <see cref="IServiceLocator"/> interface
	/// to be used by the Composite Application Library.
	/// </summary>
	public class WindsorServiceLocator : ServiceLocatorImplBase
	{
		private readonly IWindsorContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindsorServiceLocator"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public WindsorServiceLocator(IWindsorContainer container)
		{
			this.container = container;
		}

		/// <summary>
		///             When implemented by inheriting classes, this method will do the actual work of resolving
		///             the requested service instance.
		/// </summary>
		/// <param name="serviceType">Type of instance requested.</param>
		/// <param name="key">Name of registered service you want. May be null.</param>
		/// <returns>
		/// The requested service instance.
		/// </returns>
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

		/// <summary>
		///             When implemented by inheriting classes, this method will do the actual work of
		///             resolving all the requested service instances.
		/// </summary>
		/// <param name="serviceType">Type of service requested.</param>
		/// <returns>
		/// Sequence of service instance objects.
		/// </returns>
		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return (object[])container.ResolveAll(serviceType);
		}
	}
}