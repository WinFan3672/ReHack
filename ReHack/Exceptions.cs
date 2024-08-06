namespace ReHack.Exceptions
{
	/// <summary>
	/// An exception that causes the SSH client to display an error message instead of a stack trace.
	/// </summary>	
	public class ErrorMessageException : Exception
	{
		/// 
		public ErrorMessageException() {}
		///
		public ErrorMessageException(string Message) : base(Message) {}
	}

}
