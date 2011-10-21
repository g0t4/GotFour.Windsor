namespace GotFour.Windsor.Testing
{
	using System;

	public class ContainerValidationException : ApplicationException
	{
		public ContainerValidationException(string message) : base(message)
		{
		}
	}
}