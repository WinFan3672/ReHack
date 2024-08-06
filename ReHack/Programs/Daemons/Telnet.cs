using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;
using ReHack.Exceptions;

namespace ReHack.Programs.Daemons.Telnet
{
	/// <summary>Telnet daemon.</summary>
	public static class TelnetDaemon
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 1 && Args.Contains("start"))
			{
				if (NodeUtils.CheckPort(Client, "telnet"))
				{
					throw new ErrorMessageException("Daemon already running");
				}
				Client.AddPort("telnet");
				return true;
			}
			else if (Args.Length == 1 && Args.Contains("stop"))
			{
				if (!NodeUtils.CheckPort(Client, "telnet"))
				{
					throw new ErrorMessageException("Daemon not running");
				}
				Client.RemovePort("telnet");
				return true;
			}
			else
			{
				AnsiConsole.MarkupLine("[bold blue]usage[/]: telnetd [[start|stop]]");
				return false;
			}
		}
	}
}
