namespace GotFour.Windsor
{
	using Castle.MicroKernel.Registration;

	public abstract class ExtendedRegistryBase : RegistryBase
	{
		public BasedOnDescriptor ScanMyAssemblyFor<T>()
		{
			return FromAssemblyContaining(GetType()).BasedOn<T>();
		}
	}
}