using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Exceptions;
using Spectre.Console;

namespace ReHack.Programs.Deamons.C2
{
	/// <summary>Command-and-control daemon.</summary>
	public static class C2D
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (NodeUtils.CheckPort(Client, "c2"))
			{
				throw new ErrorMessageException("The deamon is already running");
			}
			Client.AddPort("c2");
			AnsiConsole.MarkupLine("[green]Success[/]! The command-and-control deamon is running!");
			AnsiConsole.MarkupLine("You can now use compatible software to administer the server and add clients.");
			return true;
		}
	}
}
