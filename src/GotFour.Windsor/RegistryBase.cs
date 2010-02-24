namespace GotFour.Windsor
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;

	public class RegistryBase : IWindsorInstaller
	{
		public RegistryBase()
		{
			_Registrations = new List<IRegistration>();
		}

		protected List<IRegistration> _Registrations { get; set; }

		public virtual void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(_Registrations.ToArray());
		}

		public virtual ComponentRegistration<S> For<S>()
		{
			var registration = Component.For<S>();
			_Registrations.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F>()
		{
			var registration = Component.For<S, F>();
			_Registrations.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F1, F2>()
		{
			var registration = Component.For<S, F1, F2>();
			_Registrations.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F1, F2, F3>()
		{
			var registration = Component.For<S, F1, F2, F3>();
			_Registrations.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F1, F2, F3, F4>()
		{
			var registration = Component.For<S, F1, F2, F3, F4>();
			_Registrations.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration For(params Type[] types)
		{
			var registration = Component.For(types);
			_Registrations.Add(registration);
			return registration;
		}

		public FromAssemblyDescriptor FromAssemblyNamed(string assemblyName)
		{
			var descriptor = AllTypes.FromAssemblyNamed(assemblyName);
			_Registrations.Add(descriptor);
			return descriptor;
		}

		public FromAssemblyDescriptor FromAssembly(Assembly assembly)
		{
			var descriptor = AllTypes.FromAssembly(assembly);
			_Registrations.Add(descriptor);
			return descriptor;
		}

		public FromAssemblyDescriptor FromAssemblyContaining<T>()
		{
			var descriptor = AllTypes.FromAssemblyContaining<T>();
			_Registrations.Add(descriptor);
			return descriptor;
		}

		public FromAssemblyDescriptor FromAssemblyContaining(Type type)
		{
			var descriptor = AllTypes.FromAssemblyContaining(type);
			_Registrations.Add(descriptor);
			return descriptor;
		}
	}
}