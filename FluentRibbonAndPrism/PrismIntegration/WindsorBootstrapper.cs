using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Castle.Core;
using Castle.Windsor;
using log4net;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.ServiceLocation;

namespace FluentRibbonAndPrism.PrismIntegration
{
	/// <summary>
	/// Bootloader that uses an IWindsorContainer for the inversion of control aspects 
	/// and Log4Net for the logging mechanism.
	/// </summary>
	public abstract class WindsorBootstrapper
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(WindsorBootstrapper));

		public IWindsorContainer Container { get; private set; }

		protected virtual ILog Logger
		{
			get { return logger; }
		}

		/// <summary>
		/// Runs the bootstrapper process.
		/// </summary>
		public virtual void Run()
		{
			logger.Info("Creating Windsor container");
			Container = new WindsorContainer();

			logger.Info("Configuring container");

			ConfigureContainer();

			logger.Info("Running initialization procedures");

			AfterContainerConfigured();

			logger.Info("Configuring region adapters");

			ConfigureRegionAdapterMappings();
			ConfigureDefaultRegionBehaviors();
			logger.Info("Creating shell");

			BeforeShellCreated();

			DependencyObject shell = CreateShell();

			if (shell != null)
			{
				var manager = Container.Resolve<IRegionManager>();
				RegionManager.SetRegionManager(shell, manager);
				RegionManager.UpdateRegions();
			}

			logger.Info("Initializing modules");
			InitializeModules();

			logger.Info("Bootstrapper sequence completed");

			AfterShellCreated();
		}

		protected virtual void AfterContainerConfigured() { }

		protected virtual void BeforeShellCreated() { }

		protected virtual void AfterShellCreated() { }

		/// <summary>
		/// Configures the <see cref="IWindsorContainer"/>. May be overwritten in a derived class to add specific
		/// type mappings required by the application.
		/// </summary>
		protected virtual void ConfigureContainer()
		{
			Container.AddComponent<ILoggerFacade, Log4NetLogger>();
			Container.Kernel.AddComponentInstance(typeof(IWindsorContainer).FullName, typeof(IWindsorContainer), Container);

			IModuleCatalog moduleCatalog = GetModuleCatalog();
			if (moduleCatalog != null)
			{
				Container.Kernel.AddComponentInstance(typeof(IModuleCatalog).FullName, typeof(IModuleCatalog), moduleCatalog);

				// All modules are registered in Container.
				foreach (var moduleInfo in moduleCatalog.Modules)
				{
					Container.AddComponent(moduleInfo.ModuleName, Type.GetType(moduleInfo.ModuleType));
				}
			}

			Container.AddComponent<IServiceLocator, WindsorServiceLocator>();
			Container.AddComponent<IModuleInitializer, ModuleInitializer>();
			Container.AddComponent<IModuleManager, ModuleManager>();
			Container.AddComponent<RegionAdapterMappings, RegionAdapterMappings>();
			Container.AddComponent<IRegionManager, RegionManager>();
			Container.AddComponent<IRegionViewRegistry, RegionViewRegistry>();
			Container.AddComponent<IRegionBehaviorFactory, RegionBehaviorFactory>();

			Container.AddComponentLifeStyle<DelayedRegionCreationBehavior>(LifestyleType.Transient);

			ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());
		}

		/// <summary>
		/// Configures the default region adapter mappings to use in the application, in order
		/// to adapt UI controls defined in XAML to use a region and register it automatically.
		/// May be overwritten in a derived class to add specific mappings required by the application.
		/// </summary>
		/// <returns>The <see cref="RegionAdapterMappings"/> instance containing all the mappings.</returns>
		protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var regionAdapterMappings = Container.Resolve<RegionAdapterMappings>();
			if (regionAdapterMappings != null)
			{
				Container.AddComponent<SelectorRegionAdapter>();
				Container.AddComponent<ItemsControlRegionAdapter>();
				regionAdapterMappings.RegisterMapping(typeof(Selector), Container.Resolve<SelectorRegionAdapter>());
				regionAdapterMappings.RegisterMapping(typeof(ItemsControl), Container.Resolve<ItemsControlRegionAdapter>());
			}

			return regionAdapterMappings;
		}

		/// <summary>
		/// Initializes the modules. May be overwritten in a derived class to use custom 
		/// module loading and avoid using an <seealso cref="IModuleManager"/> and 
		/// <seealso cref="IModuleCatalog"/>.
		/// </summary>
		protected virtual void InitializeModules()
		{
			var moduleManager = Container.Resolve<IModuleManager>();
			if (moduleManager == null)
			{
				throw new InvalidOperationException("The IModuleLoader is required and cannot be null in order to initialize the modules.");
			}
			moduleManager.Run();
		}

		/// <summary>
		/// Configures the <see cref="IRegionBehaviorFactory"/>. This will be the list of default
		/// behaviors that will be added to a region. 
		/// </summary>
		protected virtual void ConfigureDefaultRegionBehaviors()
		{
			var defaultRegionBehaviors = Container.Resolve<IRegionBehaviorFactory>();
			defaultRegionBehaviors.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey,
															  typeof(AutoPopulateRegionBehavior));

			defaultRegionBehaviors.AddIfMissing(BindRegionContextToDependencyObjectBehavior.BehaviorKey,
															  typeof(BindRegionContextToDependencyObjectBehavior));

			defaultRegionBehaviors.AddIfMissing(RegionActiveAwareBehavior.BehaviorKey,
															  typeof(RegionActiveAwareBehavior));

			defaultRegionBehaviors.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey,
															  typeof(SyncRegionContextWithHostBehavior));

			defaultRegionBehaviors.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey,
															  typeof(RegionManagerRegistrationBehavior));

			return;
		}

		/// <summary>
		/// Returns the module enumerator that will be used to initialize the modules.
		/// </summary>
		/// <remarks>
		/// When using the default initialization behavior, this method must be overwritten by a derived class.
		/// </remarks>
		/// <returns>An instance of <see cref="IModuleCatalog"/> that will be used to initialize the modules.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		protected virtual IModuleCatalog GetModuleCatalog()
		{
			return null;
		}

		/// <summary>
		/// Creates the shell or main window of the application.
		/// </summary>
		/// <returns>The shell of the application.</returns>
		/// <remarks>
		/// If the returned instance is a <see cref="DependencyObject"/>, the
		/// <see cref="WindsorBootstrapper"/> will attach the default <seealso cref="IRegionManager"/> of
		/// the application in its <see cref="RegionManager.RegionManagerProperty"/> attached property
		/// in order to be able to add regions by using the <seealso cref="RegionManager.RegionNameProperty"/>
		/// attached property from XAML.
		/// </remarks>
		protected abstract DependencyObject CreateShell();
	}
}