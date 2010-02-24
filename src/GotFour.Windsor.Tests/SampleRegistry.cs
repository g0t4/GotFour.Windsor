namespace GotFour.Windsor.Tests
{
	using Castle.Facilities.Startable;
	using Castle.MicroKernel.Registration;

	public class SampleRegistry : RegistryBase
	{
		public SampleRegistry()
		{
			// Register a singleton
			For<IFoo>().ImplementedBy<Foo>().LifeStyle.Singleton(); // Extension methods to call property.

			// Register a single item
			For<IFoo>().ImplementedBy<Foo>();
			For(typeof (IFoo)).ImplementedBy<Foo>();
			AddComponent<IFoo, Foo>();

			// Custom actions if you want to access the original container API, with deferred installation via lambda expressions
			Custom(c => c.AddComponent<IFoo, Foo>());
			Custom(c => c.Register(Component.For<IFoo>().ImplementedBy<Foo>()));

			// Scan for types
			FromAssemblyContaining<SampleRegistry>().BasedOn<IFoo>();
			FromAssemblyContaining(typeof (SampleRegistry)).BasedOn<IFoo>();
			FromAssemblyNamed("GotFour.Windsor.Tests").BasedOn<IFoo>();
			FromAssembly(typeof (SampleRegistry).Assembly).BasedOn<IFoo>();

			// Forwarding types
			For<IFoo, Foo>().ImplementedBy<Foo>();
			For<IFoo, Foo, FooBar>().ImplementedBy<FooBar>();
			For<IFoo, Foo, FooBar, FooBar2>().ImplementedBy<FooBar2>();
			For<IFoo, Foo, FooBar, FooBar2, FooBar3>().ImplementedBy<FooBar3>();

			// Adding facilities
			AddFacility<StartableFacility>();
		}
	}

	public class SampleExtendedRegistry : ExtendedRegistryBase
	{
		public SampleExtendedRegistry()
		{
			// Same as scanning above in SampleRegistry but much cleaner!
			ScanMyAssemblyFor<IFoo>();

			// Scan for all services of the pattern Service : IService
			ScanMyAssembly(Conventions.FirstInterfaceIsIName);

			// Scan for all services of the pattern Whatever : IService (register with first interface)
			ScanMyAssembly(Conventions.FirstInterface);

			// Next we could use some attributes to discover services, to register imports / exports :)
		}
	}
}