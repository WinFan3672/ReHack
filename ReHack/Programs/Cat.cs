using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Filesystem;
using ReHack.Exceptions;

namespace ReHack.Programs.Cat
{
	/// <summary>Prints the contents of files.</summary>
	public static class Cat
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 0)
			{
				Console.WriteLine("usage: cat [file]");
				return false;
			}
			try
			{
				VirtualFile File = Client.Root.GetFile(Args[0]) ?? throw new ErrorMessageException("File not found");
				Console.WriteLine(File.Content);
				return true;
			}
			catch (NullReferenceException)
			{
				Console.WriteLine("error: Invalid file");
				return false;
			}
		}
	}
}
