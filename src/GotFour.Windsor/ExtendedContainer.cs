namespace GotFour.Windsor
{
	using Castle.Core;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Releasers;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor;
	using CommonServiceLocator.WindsorAdapter;
	using Microsoft.Practices.ServiceLocation;

	///<summary>
	///	Additions to a base windsor container to:
	///	-inject the CommonServiceLocator
	///	-set the release policy to NoTracking
	///	-set transient as the default lifestyle
	///	-add the list sub resolver
	/// -track static instance of container
	///</summary>
	public class ExtendedContainer : WindsorContainer
	{
		public static ExtendedContainer Instance { get; set; }

		public ExtendedContainer()
		{
			Instance = this;
			InjectGod();
			AddListResolver();
			SetTransientAsDefaultLifestyle();
			SetReleasePolicy();
			RegisterCommonServiceLocator();
		}

		protected virtual void SetReleasePolicy()
		{
			Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
		}

		protected virtual void InjectGod()
		{
			Register(Component.For<IWindsorContainer>().Instance(this).LifeStyle.Singleton);
		}

		protected virtual void SetTransientAsDefaultLifestyle()
		{
			Kernel.ComponentModelCreated += Kernel_ComponentModelCreated;
		}

		protected virtual void RegisterCommonServiceLocator()
		{
			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(this));
			Register(Component.For<IServiceLocator>().Instance(ServiceLocator.Current).LifeStyle.Singleton);
		}

		protected virtual void Kernel_ComponentModelCreated(ComponentModel model)
		{
			if (model.LifestyleType == LifestyleType.Undefined)
			{
				model.LifestyleType = LifestyleType.Transient;
			}
		}

		protected virtual void AddListResolver()
		{
			Kernel.Resolver.AddSubResolver(new ListResolver(Kernel));
		}

		public virtual void Reset()
		{
			Instance = new ExtendedContainer();
		}
	}
}