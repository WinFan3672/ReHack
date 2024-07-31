using ReHack.Node;
using ReHack.Node.WebServer;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.W3
{
	public static class W3
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 0)
			{
				Console.WriteLine("usage: w3 [hostname]");
				return false;
			}
			if (!NodeUtils.CheckNodeByAddress(Args[0]))
			{
				Console.WriteLine("error: Invalid hostname");
				return false;
			}
			if (!NodeUtils.CheckPort(NodeUtils.GetNodeByAddress(Args[0]), "http"))
			{
				Console.WriteLine("error: Connection refused");
				return false;
			}

			WebServer Server = NodeUtils.GetNodeByAddress(Args[0]) as WebServer;
			Server.Render(Client);
			return true;
		}
	}
}
