namespace GotFour.Windsor.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class ExtendedRegistryBaseTests : RegistryTestFixureBase
	{
		[Test]
		public void ScanMyAssemblyFor_WithIFoo_ResolvesAll()
		{
			var extendedRegistry = new ExtendedRegistryTest();

			extendedRegistry.ScanMyAssemblyFor<IFoo>();

			var container = InstallInContainer(extendedRegistry);
			VerifyAll(container);
		}

		[Test]
		public void ScanMyAssemblyWithAction_RegisterWithFirstInterface_ResolvesFoo()
		{
			var extendedRegistry = new ExtendedRegistryTest();

			extendedRegistry.ScanMyAssembly(Conventions.FirstInterfaceIsIName);

			var container = InstallInContainer(extendedRegistry);
			var foos = container.ResolveAll<IFoo>();
			Expect(foos, Has.Some.TypeOf<Foo>());
			Expect(foos, Has.No.TypeOf<FooBar>());
		}

		[Test]
		public void ScanMyAssembly_RegisterFirstInterface_ResolvesAllFoos()
		{
			var extendedRegistry = new ExtendedRegistryTest();

			extendedRegistry.ScanMyAssembly(Conventions.FirstInterface);

			var container = InstallInContainer(extendedRegistry);
			VerifyAll(container);
		}
	}
}