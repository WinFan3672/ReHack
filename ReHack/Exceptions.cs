namespace ReHack.Exceptions
{
	public class ErrorMessageException : Exception
	{
		/// <summary>
		/// An exception that causes the SSH client to display an error message instead of a stack trace.
		/// </summary>
		
		public ErrorMessageException() {}
		public ErrorMessageException(string Message) : base(Message) {}
	}

}
