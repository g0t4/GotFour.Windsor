namespace GotFour.Windsor
{
	using System;
	using Castle.MicroKernel.Registration;

	public abstract class ExtendedRegistryBase : RegistryBase
	{
		public BasedOnDescriptor ScanMyAssemblyFor<T>()
		{
			return FromMyAssembly().BasedOn<T>();
		}

		public BasedOnDescriptor ScanMyAssemblyFor(Type type)
		{
			return FromMyAssembly().BasedOn(type);
		}

		private FromAssemblyDescriptor FromMyAssembly()
		{
			return FromAssemblyContaining(GetType());
		}

		public BasedOnDescriptor ScanMyAssembly(Func<Type, BasedOnDescriptor> convention)
		{
			var registration = convention(this.GetType());
			Steps.Add(registration);
			return registration;
		}

		public BasedOnDescriptor ScanAssemblyContaining<T>(Func<Type, BasedOnDescriptor> convention)
		{
			var item = convention(typeof(T));
			Steps.Add(item);
			return item;
		}
	}
}