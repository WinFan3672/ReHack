using ReHack.Data;
using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.Nmap
{
	public static class Nmap
	{
		public static void ShowPorts(BaseNode Client)
		{
			if (Client.Ports.Count == 0)
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: No open ports found");
				return;
			}
			PrintUtils.Divider();
			AnsiConsole.MarkupLine("Num\tState\tService");
			PrintUtils.Divider();
			foreach(Port Item in Client.Ports)
			{
				if (Item.Open)
				{
					AnsiConsole.MarkupLine($"[green]{Item.PortNumber}\tOpen\t{Item.ServiceName}[/]");
				}
				else
				{
					AnsiConsole.MarkupLine($"[red]{Item.PortNumber}\tClosed\t{Item.ServiceName}[/]");

				}
			}
			PrintUtils.Divider();
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 1)
			{
				BaseNode Host;
				try
				{
					Host = NodeUtils.GetNodeByAddress(Args[0]);
					ShowPorts(Host);
					return true;
				}
				catch
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Invalid hostname");
					return false;
				}
			}
			else
			{
				AnsiConsole.MarkupLine("[blue]usage[/]: nmap [[hostname]]");
				return false;
			}
		}
	}
}
