using ReHack.Node;
using ReHack.Node.FTP;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.FTP
{
	/// <summary>FTP client.</summary>
	public static class FTPClient
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length != 1)
			{
				AnsiConsole.MarkupLine("[bold blue]usage[/]: ftp [[hostname]]");
				return false;
			}

			if (!NodeUtils.CheckNodeByAddress(Args[0]))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Invalid hostname");
				return false;
			}

			FTPServer? Target = NodeUtils.GetNodeByAddress(Args[0]) as FTPServer;

			if (Target == null || !NodeUtils.CheckPort(Target, "ftp"))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Connection refused");
				return false;
			}

			if (!Target.Anonymous && !Target.Authenticate(Target.GetUser("ftpuser")) || !Target.CheckAccessControl(Client))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Access denied");
			}

			Target.Folder.View(Client);

			return true;
		}
	}
}
