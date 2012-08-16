namespace GotFour.Windsor
{
	using System;
	using Castle.Core.Internal;
	using Castle.MicroKernel.Registration;

	public abstract class ExtendedRegistryBase : RegistryBase
	{
		public BasedOnDescriptor ScanMyAssemblyFor<T>()
		{
			return ScanMyAssemblyFor(typeof (T));
		}

		public BasedOnDescriptor ScanMyAssemblyFor(Type type)
		{
			return FromMyAssembly().BasedOn(type).WithService.Base();
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

		public BasedOnDescriptor ScanMyAssemblyForSelfService()
		{
			return ScanMyAssembly(IsSelfService);
		}

		public BasedOnDescriptor IsSelfService(Type typeInAssembly)
		{
			return
				AllTypes
					.FromAssembly(typeInAssembly.Assembly)
					.Pick()
					.If(t => t.IsConcrete() && !t.IsGenericType && t.HasAttribute<SelfServiceAttribute>())
					.WithService.Self();
		}
	}
}