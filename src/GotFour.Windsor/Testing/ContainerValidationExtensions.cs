namespace GotFour.Windsor.Testing
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Castle.Core;
	using Castle.MicroKernel;
	using Castle.MicroKernel.SubSystems.Naming;
	using Castle.Windsor;

	public static class ContainerValidationExtensions
	{
		/// <summary>
		/// 	Check that the container has a service registered for TService with implementation TImplementation
		/// 	<exception cref = "ContainerValidationException">If the registration is missing</exception>
		/// </summary>
		public static void ValidateHasRegistration<TService, TImplementation>(this IWindsorContainer container)
		{
			var hasRegistration = container.Kernel.GetAssignableHandlers(typeof (TService)).Any(
				h => h.ComponentModel.Implementation == typeof (TImplementation));
			if (hasRegistration)
			{
				return;
			}
			var message = string.Format("Handler for service {0} with implementation {1} was not registered", typeof (TService),
			                            typeof (TImplementation));
			throw new ContainerValidationException(message);
		}

		/// <summary>
		/// 	Check that all registrations are valid
		/// 	This is a great thing to run after application intialization to fail fast
		/// 	<exception cref = "ContainerValidationException">If any registrations are missing dependencies</exception>
		/// </summary>
		public static void ExpectAllRegistrationsAreValid(this IWindsorContainer container)
		{
			// note: adapted from a future release of windsor's PotentiallyMisconfiguredComponents
			var items = GetPotentiallyMisconfiguredComponents(container);
			if (!items.Any())
			{
				return;
			}
			var messages = items.Select(i => GetMissingDependenciesMessage(i, container.Kernel)).ToArray();
			var missing = string.Join(Environment.NewLine, messages);
			throw new ContainerValidationException("Registrations pending: " + Environment.NewLine + missing);
		}

		private static string GetMissingDependenciesMessage(HandlerByKeyDebuggerView item, IKernel kernel)
		{
			var missing = string.Empty;
			var dependencies = item.Service.ComponentModel
				.Constructors
				.SelectMany(c => c.Dependencies)
				.Where(d => DependencyPending(d, kernel));


			foreach (var dependency in dependencies)
			{
				missing += string.Format("\tdependency with key {0} : {1}", dependency.DependencyKey, dependency.TargetType) +
				           Environment.NewLine;
			}
			return string.Format("For key {0}: {1}", item.Key, item.ServiceString) + Environment.NewLine + missing;
		}

		private static bool DependencyPending(DependencyModel dependency, IKernel kernel)
		{
			var handler = kernel.GetHandler(dependency.TargetType);
			return handler == null || handler.CurrentState == HandlerState.WaitingDependency;
		}

		private static HandlerByKeyDebuggerView[] GetPotentiallyMisconfiguredComponents(IWindsorContainer container)
		{
			var naming = container.Kernel.GetSubSystem(SubSystemConstants.NamingKey) as INamingSubSystem;

			var waitingComponents = naming.GetKey2Handler()
				.Where(h => h.Value.CurrentState == HandlerState.WaitingDependency)
				.ToArray();
			if (waitingComponents.Length == 0)
			{
				return new HandlerByKeyDebuggerView[0];
			}
			return new HandlersByKeyDictionaryDebuggerView(waitingComponents).Items;
		}
	}

	public class HandlersByKeyDictionaryDebuggerView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly HandlerByKeyDebuggerView[] items;

		public HandlersByKeyDictionaryDebuggerView(IEnumerable<KeyValuePair<string, IHandler>> key2Handler)
		{
			items = key2Handler.Select(h => new HandlerByKeyDebuggerView(h.Key, h.Value)).ToArray();
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count
		{
			get { return items.Length; }
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public HandlerByKeyDebuggerView[] Items
		{
			get { return items; }
		}
	}

	public class HandlerByKeyDebuggerView
	{
		public HandlerByKeyDebuggerView(string key, IHandler service)
		{
			Key = key;
			Service = service;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string Key { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public IHandler Service { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string ServiceString
		{
			get
			{
				var value = Service.Service.Name;
				var impl = Service.ComponentModel.Implementation;
				if (impl == Service.Service)
				{
					return value;
				}
				value += " / ";
				if (impl == null)
				{
					value += "no type";
				}
				else
				{
					value += impl.Name;
				}
				return value;
			}
		}

		public IEnumerable<DependencyModel> Dependencies()
		{
			return Service.ComponentModel.Dependencies;
		}
	}
}