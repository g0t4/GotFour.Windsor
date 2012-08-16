namespace GotFour.Windsor.Testing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Handlers;
	using Castle.Windsor;
	using Castle.Windsor.Diagnostics;
	using Castle.Windsor.Diagnostics.DebuggerViews;

	public static class ContainerValidationExtensions
	{
		/// <summary>
		/// 	Check that the container has a service registered for TService with implementation TImplementation
		/// 	<exception cref="ContainerValidationException">If the registration is missing</exception>
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
		/// 	<exception cref="ContainerValidationException">If any registrations are missing dependencies</exception>
		/// </summary>
		public static void ExpectAllRegistrationsAreValid(this IWindsorContainer container)
		{
			var items = GetPotentiallyMisconfiguredComponents(container);
			if (!items.Any())
			{
				return;
			}
			var messages = items.Select(i => i.Message);
			var missing = string.Join(Environment.NewLine, messages);
			throw new ContainerValidationException("Registrations pending: " + Environment.NewLine + missing);
		}

		private static IEnumerable<ComponentStatusDebuggerViewItem> GetPotentiallyMisconfiguredComponents(IWindsorContainer container)
		{
			var host = container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey) as IDiagnosticsHost;
			return host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>()
				.Inspect()
				.OfType<IExposeDependencyInfo>()
				.Select(h => new ComponentStatusDebuggerViewItem(h));
		}
	}
}