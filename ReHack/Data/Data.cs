using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Filesystem;

namespace ReHack.Data
{
	/// <summary>Static class containing core game data.</summary>
	public static class GameData {
		/// <summary>The node list. This is where you need to add a node for it to be accessible to the other nodes.</summary>
		public static List<BaseNode> Nodes { get; } = new List<BaseNode>();

		/// <summary></summary>
		public static Random Rand = new Random();

		/// <summary>Every port the game understands.</summary>
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

		/// <summary>Adds a node to GameData.Nodes </summary>
		public static BaseNode AddNode(BaseNode Node)
		{
			Nodes.Add(Node);
			return Node;
		}

		/// <summary>Returns a port from GameData.Ports</summary>
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

		/// <summary>Adds a port to a node.</summary>
		public static void AddPort(BaseNode Node, string PortID)
		{
			Port P = GetPort(PortID);
			Node.Ports.Add(P);
		}

		/// <summary>Debugging function - prints all nodes </summary>
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

		/// <summary>Usernames that the game does not allow the player to create.</summary>
		public static List<string> BannedUsernames = new List<string> { "admin", "root", "" };

		private static string PasswordsRaw = FileUtils.GetFileContents("Passwords.txt") ?? throw new ArgumentException("Passwords file not found");

		/// <summary>Brute-force dictionary.</summary>
		public static string[] Passwords = PasswordsRaw.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);

		/// <summary>The programs added to every BaseNode.</summary>
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
				"ping",
				"telnet",
				"ftp",
				"bankutil",
		};

		/// <summary>The manpages added to every BaseNode.</summary>
		public static List<string> DefaultManpages = new List<string> {
		};

		/// <summary>The default directories added to every BaseNode's filesystem.</summary>
		public static VirtualDirectory[] DefaultDirs = new VirtualDirectory[] {
			new VirtualDirectory("home", new VirtualFile[]{}, new VirtualDirectory[] {}),
				new VirtualDirectory("etc", new VirtualFile[] {}, new VirtualDirectory[] {
						new VirtualDirectory("apt", new VirtualFile[] {new VirtualFile("sources.list", "pkg.debian.org")}, new VirtualDirectory[] {}),
						}),
				new VirtualDirectory("var", new VirtualFile[]{}, new VirtualDirectory[]{}),
				new VirtualDirectory("srv", new VirtualFile[]{}, new VirtualDirectory[] {
						new VirtualDirectory("ftp", new VirtualFile[]{}, new VirtualDirectory[]{}),
						}),
		};

	}
}
