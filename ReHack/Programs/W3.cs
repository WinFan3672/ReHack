using ReHack.Node;
using ReHack.Node.Webserver;
using ReHack.BaseMethods;

namespace ReHack.Programs.W3
{
	/// <summary>Primitive web browser (more like a webpage viewer)</summary>
	public static class W3
	{
		/// <summary>Program function.</summary>
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

			WebServer Server = NodeUtils.GetNodeByAddress(Args[0]) as WebServer ?? throw new ArgumentException("Invalid hostname");
			Server.Render(Client);
			return true;
		}
	}
}
