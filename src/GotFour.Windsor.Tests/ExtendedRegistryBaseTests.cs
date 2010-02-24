namespace GotFour.Windsor.Tests
{
	using Castle.MicroKernel.Registration;
	using NUnit.Framework;

	[TestFixture]
	public class ExtendedRegistryBaseTests : RegistryTestFixureBase
	{
		[Test]
		public void ScanMyAssemblyFor_WithIFoo_ResolvesAll()
		{
			var extendedRegistry = new ExtendedRegistryTest();

			extendedRegistry.ScanMyAssemblyFor<IFoo>();

			VerifyAll(extendedRegistry);
		}
	}

	public class ExtendedRegistryTest : ExtendedRegistryBase
	{
		public BasedOnDescriptor ScanMyAssemblyFor<T>()
		{
			return FromAssemblyContaining(GetType()).BasedOn<T>();
		}
	}
}