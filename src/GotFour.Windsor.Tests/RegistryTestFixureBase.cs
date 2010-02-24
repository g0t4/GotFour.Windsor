namespace GotFour.Windsor.Tests
{
	using Castle.Windsor;
	using NUnit.Framework;

	public class RegistryTestFixureBase : AssertionHelper
	{
		protected void Verify<S, T>(IWindsorContainer container)
		{
			var service = container.Resolve<S>();
			Expect(service, Is.Not.Null.And.TypeOf<T>());
		}

		protected void VerifyAll(IWindsorContainer container)
		{
			var services = container.ResolveAll<IFoo>();
			Expect(services, Has.Some.TypeOf<Foo>());
			Expect(services, Has.Some.TypeOf<FooBar>());
			Expect(services, Has.Some.TypeOf<FooBar2>());
			Expect(services, Has.Some.TypeOf<FooBar3>());
			Expect(services, Has.Some.TypeOf<FooBar4>());
		}

		protected static WindsorContainer InstallInContainer(IWindsorInstaller registry)
		{
			var container = new WindsorContainer();
			container.Install(registry);
			return container;
		}
	}
}