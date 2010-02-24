namespace GotFour.Windsor
{
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Registration.Lifestyle;

	public static class RegistrationExtensions
	{
		public static BasedOnDescriptor Exclude<T>(this BasedOnDescriptor descriptor)
		{
			return descriptor.Unless(t => t == typeof (T));
		}

		public static ComponentRegistration<S> Singleton<S>(this LifestyleGroup<S> group)
		{
			return group.Singleton;
		}

		public static ComponentRegistration<S> Transient<S>(this LifestyleGroup<S> group)
		{
			return group.Transient;
		}

		public static ComponentRegistration<S> Pooled<S>(this LifestyleGroup<S> group)
		{
			return group.Pooled;
		}

		public static ComponentRegistration<S> PerThread<S>(this LifestyleGroup<S> group)
		{
			return group.PerThread;
		}

		public static ComponentRegistration<S> PerWebRequest<S>(this LifestyleGroup<S> group)
		{
			return group.PerWebRequest;
		}
	}
}