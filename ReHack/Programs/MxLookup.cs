using ReHack.Node;
using ReHack.Node.Mail;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.MxLookup
{
	public static class MXLookup
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length != 1)
			{
				AnsiConsole.MarkupLine("[bold blue]usage[/]: mxlookup [[hostname]]");
				return false;
			}

			if (!NodeUtils.CheckNodeByAddress(Args[0]))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Invalid hostname");
			}
			
			MailServer? Node = NodeUtils.GetNodeByAddress(Args[0]) as MailServer;

			if (Node == null || !NodeUtils.CheckPort(Node, "smtp"))
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: Connection refused");
			}

			List<string> Accounts = Node.Lookup(Client);

			foreach(string Account in Accounts)
			{
				AnsiConsole.MarkupLine($"[yellow]{Account}[/][blue]@[/][yellow]{Node.Address}[/]");
			}

			if (Accounts.Count == 0)
			{
				AnsiConsole.MarkupLine("[bold red]error[/]: No accounts found");
			}

			return true;
		}
	}
}
