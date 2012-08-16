namespace GotFour.Windsor.Tests.Testing
{
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using NUnit.Framework;
	using Windsor.Testing;

	[TestFixture]
	public class ContainerTestExtensionsTesting : AssertionHelper
	{
		private class Independent
		{
		}

		private class IndependentExtended : Independent
		{
		}

		private class Dependent
		{
			private readonly Independent _Independent;

			public Dependent(Independent independent)
			{
				_Independent = independent;
			}
		}

		[Test]
		[ExpectedException(typeof (ContainerValidationException))]
		public void ValidateHasRegistration_NoRegistration_ThrowsException()
		{
			var container = new WindsorContainer();

			container.ValidateHasRegistration<Independent, Independent>();
		}

		[Test]
		public void ValidateHasRegistration_HasRegistration_NoException()
		{
			var container = new WindsorContainer();
			container.Register(Component.For<Independent>());

			container.ValidateHasRegistration<Independent, Independent>();
		}

		[Test]
		[ExpectedException(typeof (ContainerValidationException))]
		public void ValidateHasRegistration_HasOtherImplementation_ThrowsException()
		{
			var container = new WindsorContainer();
			container.Register(Component.For<Independent>().ImplementedBy<IndependentExtended>());

			container.ValidateHasRegistration<Independent, Independent>();
		}

		[Test]
		[ExpectedException(typeof (ContainerValidationException), ExpectedMessage = @"Registrations pending: 
Some dependencies of this component could not be statically resolved.
'GotFour.Windsor.Tests.Testing.ContainerTestExtensionsTesting+Dependent' is waiting for the following dependencies:
- Service 'GotFour.Windsor.Tests.Testing.ContainerTestExtensionsTesting+Independent' which was not registered.
")]
		public void ExpectRegistrationsValid_Missing_ThrowsException()
		{
			var container = new WindsorContainer();
			container.Register(Component.For<Dependent>());

			container.ExpectAllRegistrationsAreValid();
		}

		[Test]
		public void ExpectRegistrationsValid_HasDependency_NoException()
		{
			var container = new WindsorContainer();
			container.Register(Component.For<Dependent>());
			container.Register(Component.For<Independent>());

			container.ExpectAllRegistrationsAreValid();
		}
	}
}