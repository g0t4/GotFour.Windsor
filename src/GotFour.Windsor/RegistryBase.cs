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
		protected List<object> Steps;

		public RegistryBase()
		{
			Steps = new List<object>();
		}

		public virtual void Install(IWindsorContainer container, IConfigurationStore store)
		{
			Steps.ForEach(t => ApplyStep(t, container));
		}

		private static void ApplyStep(object step, IWindsorContainer container)
		{
			if (step is Action<IWindsorContainer>)
			{
				(step as Action<IWindsorContainer>)(container);
			}
			else if (step is IRegistration)
			{
				container.Register(step as IRegistration);
			}
		}

		public virtual ComponentRegistration<S> For<S>()
		{
			var registration = Component.For<S>();
			Steps.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F>()
		{
			var registration = Component.For<S, F>();
			Steps.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F1, F2>()
		{
			var registration = Component.For<S, F1, F2>();
			Steps.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F1, F2, F3>()
		{
			var registration = Component.For<S, F1, F2, F3>();
			Steps.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration<S> For<S, F1, F2, F3, F4>()
		{
			var registration = Component.For<S, F1, F2, F3, F4>();
			Steps.Add(registration);
			return registration;
		}

		public virtual ComponentRegistration For(params Type[] types)
		{
			var registration = Component.For(types);
			Steps.Add(registration);
			return registration;
		}

		public FromAssemblyDescriptor FromAssemblyNamed(string assemblyName)
		{
			var descriptor = AllTypes.FromAssemblyNamed(assemblyName);
			Steps.Add(descriptor);
			return descriptor;
		}

		public FromAssemblyDescriptor FromAssembly(Assembly assembly)
		{
			var descriptor = AllTypes.FromAssembly(assembly);
			Steps.Add(descriptor);
			return descriptor;
		}

		public FromAssemblyDescriptor FromAssemblyContaining<T>()
		{
			var descriptor = AllTypes.FromAssemblyContaining<T>();
			Steps.Add(descriptor);
			return descriptor;
		}

		public FromAssemblyDescriptor FromAssemblyContaining(Type type)
		{
			var descriptor = AllTypes.FromAssemblyContaining(type);
			Steps.Add(descriptor);
			return descriptor;
		}

		public ComponentRegistration<S> AddComponent<S, T>() where T : S
		{
			var registration = Component.For<S>().ImplementedBy<T>();
			Steps.Add(registration);
			return registration;
		}

		public void Custom(Action<IWindsorContainer> customContainerAction)
		{
			Steps.Add(customContainerAction);
		}
	}
}