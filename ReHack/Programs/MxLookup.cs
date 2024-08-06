using ReHack.Node;
using ReHack.Node.Mail;
using ReHack.BaseMethods;
using Spectre.Console;
using ReHack.Exceptions;

namespace ReHack.Programs.MxLookup
{
	/// <summary>Looks up email addresses associated with a mail server.</summary>
	public static class MXLookup
	{
		/// <summary>Program function.</summary>
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
			
			MailServer Node = NodeUtils.GetNodeByAddress(Args[0]) as MailServer ?? throw new ErrorMessageException("Connection refused");

			if (!NodeUtils.CheckPort(Node, "smtp"))
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
