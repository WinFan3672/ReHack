using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Programs.Ping
{
	/// <summary>Ping 'implementation'</summary>
	public static class PingClient
	{
		/// <summary>Pings a host and displays info.</summary>
		public static void ServiceRunner(BaseNode Client)
		{
			Console.WriteLine($"PING {Client.Address} 56 data bytes");
			for (int i = 1; i <= 4; i++)
			{
				Console.WriteLine($"64 bytes from {Client.Address}: icmp_seq={i} ttl=64");
			}
			Console.WriteLine("4 packets transmitted, 4 received; 0% packet loss");
		}
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 1)
			{
				BaseNode Host;
				try
				{
					Host = NodeUtils.GetNodeByAddress(Args[0]);
				}
				catch 
				{
					Console.WriteLine($"ping: {Args[0]}: no address associated with hostname");
					return false;
				}
				ServiceRunner(Host);
				return true;
			}
			else
			{
				Console.WriteLine("usage: ping [hostname]");
				return false;
			}
		}
	}
}
