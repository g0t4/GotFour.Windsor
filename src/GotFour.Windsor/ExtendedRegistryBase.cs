namespace GotFour.Windsor
{
	using System;
	using Castle.MicroKernel.Registration;

	public abstract class ExtendedRegistryBase : RegistryBase
	{
		public BasedOnDescriptor ScanMyAssemblyFor<T>()
		{
			return FromAssemblyContaining(GetType()).BasedOn<T>();
		}

		public BasedOnDescriptor ScanMyAssembly(Func<Type, BasedOnDescriptor> convention)
		{
			var registration = convention(this.GetType());
			Steps.Add(registration);
			return registration;
		}
	}
}