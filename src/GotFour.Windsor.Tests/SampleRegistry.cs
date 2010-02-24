namespace GotFour.Windsor.Tests
{
	using Castle.Facilities.Startable;
	using Castle.MicroKernel.Registration;

	public class SampleRegistry : RegistryBase
	{
		public SampleRegistry()
		{
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
		}
	}
}