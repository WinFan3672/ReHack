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
				AnsiConsole.MarkupLine("[bold blue]usage[/]: mxlookup [hostname]");
				return false;
			}
			
			MailServer Node = NodeUtils.GetNodeByAddress(Args[0]) as MailServer;

			return true;
		}
	}
}
