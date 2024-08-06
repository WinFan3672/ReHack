using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;
using System.Threading;

namespace ReHack.Programs.TelnetHack
{
	/// <summary>Program that pwns Telnet by stealing passwords.</summary>
	public static class TelnetPwn
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length != 1)
			{
				AnsiConsole.MarkupLine("[bold blue]usage[/]: telnetpwn [[hostname]]");
				return false;
			}
			
			if (!NodeUtils.CheckNodeByAddress(Args[0]))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Invalid hostname");
				return false;
			}

			BaseNode Target = NodeUtils.GetNodeByAddress(Args[0]);

			if (!NodeUtils.CheckPort(Target, "telnet"))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Connection refused");
				return false;
			}

			Console.WriteLine("Begin attack...");
			Console.WriteLine("Asking server for user list [CEV-2006-4578]...");
			Thread.Sleep(500);
			if (Target.Users.Length == 0)
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: No users on node");
				return false;
			}
			AnsiConsole.MarkupLine($"Found [blue]{Target.Users.Length}[/] users.");
			foreach(User TargetUser in Target.Users)
			{
				AnsiConsole.MarkupLine($"Trying malformed commands on [blue]{TargetUser.Username}[/] [[CEV-2004-1154]]...");
				Thread.Sleep(750);
				if (TargetUser.Password == null)
				{
					AnsiConsole.MarkupLine("Password sniffing [red]failed[/]");
				}
				else
				{
					AnsiConsole.MarkupLine($"Found password: [green]{TargetUser.Password}[/]");
				}
			}
			AnsiConsole.MarkupLine("Exploitation [blue]successful[/].");

			return true;
		}
	}
}
