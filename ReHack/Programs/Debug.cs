using System.Reflection;
using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Programs.Debug
{
	/// <summary>Useful program for debugging the game.</summary>
	public static class DebugClient
	{
		/// <summary>Program function.</summary>
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
			else if (Args.Length == 1 && Args.Contains("url"))
			{
				Console.Write("URL: ");
				string URL = Console.ReadLine() ?? throw new EndOfStreamException();
				(string, string) Parts = NodeUtils.DeconstructURL(URL);
				Console.WriteLine($"Server={Parts.Item1}; Resource={Parts.Item2}");
				return true;
			}
			else if (Args.Length == 1 && Args.Contains("test"))
			{
				Console.WriteLine(FileUtils.GenerateBytes());
				return true;
			}
			else
			{
				Console.WriteLine("debug [subcommand] [args...]");
				Console.WriteLine("Subcommands:");
				Console.WriteLine("\tfiles - Lists embedded files");
				Console.WriteLine("\tpasswd - Generates random, brute-forceable password");
				return false;
			}
		}
	}
}
