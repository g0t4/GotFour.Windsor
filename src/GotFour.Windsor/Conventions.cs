namespace GotFour.Windsor
{
	using System;
	using Castle.MicroKernel.Registration;

	public static class Conventions
	{
		public static BasedOnDescriptor FirstInterfaceIsIName(Type typeInAssembly)
		{
			return
				AllTypes.Pick().FromAssembly(typeInAssembly.Assembly).If(TypesFirstInterfaceIsIName).WithService.FirstInterface();
		}

		public static BasedOnDescriptor FirstInterfaceIsIName<TInAssembly>()
		{
			return FirstInterfaceIsIName(typeof (TInAssembly));
		}

		private static bool TypesFirstInterfaceIsIName(Type type)
		{
			return IsConcreteWithInterface(type)
			       && type.GetInterfaces()[0].Name == "I" + type.Name;
		}

		private static bool IsConcreteWithInterface(Type type)
		{
			return type.IsConcrete()
					&& !type.IsGenericType
			       && type.HasInterface();
		}
	}
}