using ReHack.Data;
using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Programs.Nmap
{
	public static class Nmap
	{
		public static void ShowPorts(BaseNode Client)
		{
			if (Client.Ports.Count == 0)
			{
				Console.WriteLine("No open ports found.");
				return;
			}
			PrintUtils.Divider();
			Console.WriteLine("Num\tService");
			PrintUtils.Divider();
			foreach(Port Item in Client.Ports)
			{
				Console.WriteLine($"{Item.PortNumber}\t{Item.ServiceName}");
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
					ShowPorts(Client);
					return true;
				}
				catch
				{
					Console.WriteLine("error: Invalid hostname.");
					return false;
				}
			}
			else
			{
				Console.WriteLine("usage: nmap [hostname]");
				return false;
			}
		}
	}
}
