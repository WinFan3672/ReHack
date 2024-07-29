using System.Reflection;
using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Programs.Debug
{
	public static class DebugClient
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 1 && Args.Contains("files"))
			{
				var assembly = Assembly.GetExecutingAssembly();
				foreach (var File in assembly.GetManifestResourceNames())
				{
					Console.WriteLine(File);
				}
				return true;
			}
			else if (Args.Length == 1 && Args.Contains("passwd"))
			{
				Console.WriteLine(UserUtils.PickPassword());
				return true;
			}
			else
			{
				Console.WriteLine("debug [subcommand] [args...]");
				Console.WriteLine("Subcommands:");
				Console.WriteLine("\tfiles - Lists embedded files");
				return false;
			}
		}
	}
}
