using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Filesystem;
using Spectre.Console;

namespace ReHack.Programs.LS
{
	/// <summary>Lists contents of directories.</summary>
	public static class LS
	{
		/// <summary>Program function.</summary>
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
				AnsiConsole.MarkupLine("[blue]usage[/]: ls [[directory]]");
				return false;
			}
			VirtualDirectory Directory = Client.Root.GetDirectory(DirectoryName);
			PrintUtils.Divider();
			AnsiConsole.MarkupLine($"Contents of '{DirectoryName}'");
			PrintUtils.Divider();
			foreach (VirtualDirectory Dir in Directory.SubDirectories)
			{
				AnsiConsole.MarkupLine($"[blue]{Dir.Name}[/]");
			}
			foreach (VirtualFile File in Directory.Files)
			{
				AnsiConsole.MarkupLine(File.Name);
			}
			PrintUtils.Divider();
			return true;
		}
	}
}
