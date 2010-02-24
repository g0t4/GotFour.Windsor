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

			VerifyAll(extendedRegistry);
		}
	}
}