using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Filesystem;

namespace ReHack.Data
{
	public static class GameData {
		public static List<BaseNode> Nodes { get; } = new List<BaseNode>();

		public static List<Port> Ports {get; } = new List<Port> { 
			new Port("FTP", "ftp", 21),
				new Port("SSH", "ssh", 22),
				new Port("Mail Server (SMTP)", "smtp", 25),
				new Port("Telnet", "telnet", 23),
				new Port("Domain Name Service", "dns", 53),
				new Port("HTTP Server", "http", 80),
				new Port("Mail Server (POP3)", "pop3", 110),
				new Port("Usenet (NNTP) Server", "nntp", 119),
				new Port("NTP Time Server", "ntp", 123),
				new Port("OpenVPN Server", "vpn", 1194),
				new Port("SQL Database", "sql", 1433),
				new Port("Command and Control Server", "c2", 4444),
				new Port("IRC Server", "irc", 6667),
				new Port("BitTorrent", "torrent", 6881),
				new Port("ReHackOS Node", "rehack", 7777),
				new Port("BlueMedical Monitor Service", "medical", 8989),
				new Port("Tor Relay", "tor", 9200),
		};

		public static BaseNode AddNode(BaseNode Node)
		{
			/// <summary>
			/// Adds a node to the in-game WAN. Returns the node as well, for convenience.
			/// </summary>
			Nodes.Add(Node);
			return Node;
		}

		public static Port GetPort(string PortID)
		{
			foreach (Port Service in Ports)
			{
				if (Service.ServiceID == PortID)
				{
					return Service.Copy();
				}
			}
			throw new Exception("Invalid port");
		}

		public static void AddPort(BaseNode Node, string PortID)
		{
			Port P = GetPort(PortID);
			Node.Ports.Add(P);
		}

		public static void DebugNodes()
		{
			PrintUtils.Divider();
			Console.WriteLine("Node List");
			PrintUtils.Divider();
			foreach (var Node in Nodes)
			{
				Console.WriteLine($"{Node.UID} = {Node.Address}");
			}
			PrintUtils.Divider();
		}

		public static List<string> BannedUsernames = new List<string> { "admin", "root", "" };

		public static string[] Passwords = FileUtils.GetFileContents("Passwords.txt").Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);

		public static List<string> DefaultPrograms = new List<string> { 
			"help", 
				"motd",
				"ssh",
				"man",
				"lsman",
				"apt",
				"ls",
				"sudo",
				"cat",
				"w3",
				"telnet",
				"ftp",
		};

		public static List<string> DefaultManpages = new List<string> {
		};

		public static VirtualDirectory[] DefaultDirs = new VirtualDirectory[] {
			new VirtualDirectory("home", new VirtualFile[]{}, new VirtualDirectory[] {}),
				new VirtualDirectory("etc", new VirtualFile[] {}, new VirtualDirectory[] {
						new VirtualDirectory("apt", new VirtualFile[] {new VirtualFile("sources.list", "pkg.debian.org")}, new VirtualDirectory[] {}),
						}),
				new VirtualDirectory("var", new VirtualFile[]{}, new VirtualDirectory[]{}),
		};

	}
}
