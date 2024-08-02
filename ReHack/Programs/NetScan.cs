using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Exceptions;
using ReHack.Networks;
using Spectre.Console;

namespace ReHack.Programs.LocalNetScan
{
	public static class NetScan
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			AreaNetwork Network = Client.Network ?? throw new ErrorMessageException("No local network nodes found");
			foreach(BaseNode Node in Client.Network.Nodes)
			{
				AnsiConsole.MarkupLine($"{Node.Address}");
			}
			return true;
		}
	}
}
