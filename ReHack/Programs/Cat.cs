using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Filesystem;

namespace ReHack.Programs.Cat
{
	public static class Cat
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 0)
			{
				Console.WriteLine("usage: cat [file]");
				return false;
			}
			try
			{
				VirtualFile File = Client.Root.GetFile(Args[0]);
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
