namespace GotFour.Windsor.Tests
{
	using Castle.Windsor;
	using NUnit.Framework;

	[TestFixture]
	public class RegistryBaseTests : AssertionHelper
	{
		[Test]
		public void For_GenericEntry_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.For<IFoo>().ImplementedBy<Foo>();

			var container = InstallInContainer(registry);
			Verify<IFoo, Foo>(container);
		}

		[Test]
		public void For_GenericWithOneForward_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.For<IFoo, Foo>().ImplementedBy<Foo>();

			var container = InstallInContainer(registry);
			Verify<IFoo, Foo>(container);
			Verify<Foo, Foo>(container);
		}

		private void Verify<S, T>(IWindsorContainer container)
		{
			var service = container.Resolve<S>();
			Expect(service, Is.Not.Null.And.TypeOf<T>());
		}

		[Test]
		public void For_GenericWithTwoForwards_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.For<FooBar, Foo, IFoo>().ImplementedBy<FooBar>();

			var container = InstallInContainer(registry);
			Verify<IFoo, FooBar>(container);
			Verify<Foo, FooBar>(container);
			Verify<FooBar, FooBar>(container);
		}

		[Test]
		public void For_GenericWithThreeForwards_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.For<IFoo, Foo, FooBar, FooBar2>().ImplementedBy<FooBar2>();

			var container = InstallInContainer(registry);
			Verify<IFoo, FooBar2>(container);
			Verify<Foo, FooBar2>(container);
			Verify<FooBar, FooBar2>(container);
			Verify<FooBar2, FooBar2>(container);
		}

		[Test]
		public void For_GenericWithFourForwards_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.For<IFoo, Foo, FooBar, FooBar2, FooBar3>().ImplementedBy<FooBar3>();

			var container = InstallInContainer(registry);
			Verify<IFoo, FooBar3>(container);
			Verify<Foo, FooBar3>(container);
			Verify<FooBar, FooBar3>(container);
			Verify<FooBar2, FooBar3>(container);
			Verify<FooBar3, FooBar3>(container);
		}

		[Test]
		public void For_ParamOfTypes_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.For(typeof (IFoo)).ImplementedBy<Foo>();

			var container = InstallInContainer(registry);
			Verify<IFoo, Foo>(container);
		}

		[Test]
		public void FromAssemblyNamed_TestAssembly_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.FromAssemblyNamed("GotFour.Windsor.Tests").BasedOn<IFoo>().WithService.FirstInterface();

			VerifyAll(registry);
		}

		private void VerifyAll(RegistryTest registry)
		{
			var container = InstallInContainer(registry);
			var services = container.ResolveAll<IFoo>();
			Expect(services, Has.Some.TypeOf<Foo>());
			Expect(services, Has.Some.TypeOf<FooBar>());
			Expect(services, Has.Some.TypeOf<FooBar2>());
			Expect(services, Has.Some.TypeOf<FooBar3>());
			Expect(services, Has.Some.TypeOf<FooBar4>());
		}

		[Test]
		public void FromAssembly_TestAssembly_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.FromAssembly(this.GetType().Assembly).BasedOn<IFoo>().WithService.FirstInterface();

			VerifyAll(registry);
		}

		[Test]
		public void FromAssemblyContainingGeneric_TestAssembly_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.FromAssemblyContaining<IFoo>().BasedOn<IFoo>();

			VerifyAll(registry);
		}

		[Test]
		public void FromAssemblyContaining_TestAssembly_AddsRegistration()
		{
			var registry = new RegistryTest();

			registry.FromAssemblyContaining(typeof (IFoo)).BasedOn<IFoo>();

			VerifyAll(registry);
		}

		private static WindsorContainer InstallInContainer(IWindsorInstaller registry)
		{
			var container = new WindsorContainer();
			container.Install(registry);
			return container;
		}
	}

	public class RegistryTest : RegistryBase
	{
	}

	public class FooBar4 : FooBar3
	{
	}

	public class FooBar3 : FooBar2
	{
	}

	public class FooBar2 : FooBar
	{
	}

	public class FooBar : Foo
	{
	}

	public class Foo : IFoo
	{
	}

	public interface IFoo
	{
	}
}