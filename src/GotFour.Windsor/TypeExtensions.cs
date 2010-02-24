namespace GotFour.Windsor
{
	using System;
	using System.Linq;

	public static class TypeExtensions
	{
		public static bool IsConcrete(this Type type)
		{
			return !type.IsAbstract && !type.IsInterface;
		}

		public static bool HasInterface(this Type type)
		{
			return type.GetInterfaces().Any();
		}
	}
}