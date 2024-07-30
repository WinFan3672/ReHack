using ReHack.Node;
using ReHack.Node.WebServer;
using ReHack.BaseMethods;

namespace ReHack.Programs.Curl
{
	public static class Curl
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 1)
			{
				if (!NodeUtils.CheckNodeByAddress(Args[0]))
				{
					Console.WriteLine("error: invalid hostname");
					return false;
				}
				if (!NodeUtils.CheckPort(NodeUtils.GetNodeByAddress(Args[0]), "http"))
				{
					Console.WriteLine("error: Connection refused");
					return false;
				}
				WebServer Server = NodeUtils.GetNodeByAddress(Args[0]) as WebServer;
				Console.WriteLine(Server.Render(Client, "/"));
				return true;
			}
			else
			{
				Console.WriteLine("usage: curl [hostname]");
				return false;
			}
		}
	}
}
