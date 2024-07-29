using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Filesystem;

namespace ReHack.Programs.LS
{
	public static class LS
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			string DirectoryName;
			if (Args.Length == 1)
			{
				DirectoryName = Args[0];
			}
			else if (Args.Length == 0)
			{
				DirectoryName = "/";
			}
			else
			{
				Console.WriteLine("usage: ls [directory]");
				return false;
			}
			VirtualDirectory Directory = Client.Root.GetDirectory(DirectoryName);
			PrintUtils.Divider();
			Console.WriteLine($"Contents of '{DirectoryName}'");
			PrintUtils.Divider();
			foreach (VirtualDirectory Dir in Directory.SubDirectories)
			{
				Console.WriteLine(Dir.Name);
			}
			foreach (VirtualFile File in Directory.Files)
			{
				Console.WriteLine(File.Name);
			}
			PrintUtils.Divider();
			return true;
		}
	}
}
