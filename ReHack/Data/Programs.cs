using ReHack.BaseMethods;
using ReHack.Programs.SSH;
using ReHack.Programs.Help;
using ReHack.Programs.Man;
using ReHack.Programs.Ping;
using ReHack.Programs.Apt;
using ReHack.Programs.Debug;
using ReHack.Programs.Nmap;
using ReHack.Programs.LS;
using ReHack.Programs.Sudo;
using ReHack.Programs.Cat;
using ReHack.Programs.W3;
using ReHack.Programs.Hydra;
using ReHack.Programs.Telnet;
using ReHack.Programs.TelnetHack;
using ReHack.Programs.Welcome;
using ReHack.Programs.Tutorial;
using ReHack.Programs.FTP;
using ReHack.Programs.MxLookup;
using ReHack.Programs.MOTD;
using ReHack.Programs.LocalNetScan;
using ReHack.Programs.Daemons.Telnet;
using ReHack.Programs.Deamons.C2;
using ReHack.Programs.Metasploit;

namespace ReHack.Data.Programs
{
	public class ProgramData
	{
		public static List<ProgramDefinition> Programs = new List<ProgramDefinition> {
			new ProgramDefinition("ssh", "SSH Client", new ProgramDelegate(SSHClient.Program), new string[] { "ssh" } ),
			new ProgramDefinition("help", "Lists available programs", new ProgramDelegate(HelpClient.Program), new string[] {} ),
			new ProgramDefinition("man", "Manual viewer", new ProgramDelegate(ManClient.Program), new string[] { "man" } ),
			new ProgramDefinition("lsman", "List available manuals", new ProgramDelegate(ManClient.ListProgram), new string[] {} ),
			new ProgramDefinition("ping", "Checks if hosts are up", new ProgramDelegate(PingClient.Program), new string[] { "ping" } ),
			new ProgramDefinition("apt", "Package Manager", new ProgramDelegate(Apt.Program), new string[] { "apt" } ),
			new ProgramDefinition("debug", "Debugging tools", new ProgramDelegate(DebugClient.Program), new string[] {} ),
			new ProgramDefinition("nmap", "Enumerate ports on a device", new ProgramDelegate(Nmap.Program), new string[] {}),
			new ProgramDefinition("ls", "Lists contents of directories", new ProgramDelegate(LS.Program), new string[] {}),
			new ProgramDefinition("sudo", "Run commands as root", new ProgramDelegate(Sudo.Program), new string[] { "sudo" }),
			new ProgramDefinition("cat", "View contents of files", new ProgramDelegate(Cat.Program), new string[] {"cat" }),
			new ProgramDefinition("w3", "Web browser", new ProgramDelegate(W3.Program), new string[] { "w3" }),
			new ProgramDefinition("hydra", "SSH brute-force", new ProgramDelegate(Hydra.Program), new string[] {"hydra"}),
			new ProgramDefinition("telnet", "Telnet client", new ProgramDelegate(TelnetClient.Program), new string[] {}),
			new ProgramDefinition("telnetpwn", "Telnet password stealer", new ProgramDelegate(TelnetPwn.Program), new string[] {}),
			new ProgramDefinition("welcome", "A basic run-down of how to ReHack", new ProgramDelegate(WelcomeApp.Program), new string[] {}),
			new ProgramDefinition("tutorial", "First hacking tutorial", new ProgramDelegate(TutorialApp.Program), new string[] {}),
			new ProgramDefinition("ftp", "FTP Client", new ProgramDelegate(FTPClient.Program), new string[] {}),
			new ProgramDefinition("mxlookup", "Finds email addresses associated with a mail server", new ProgramDelegate(MXLookup.Program), new string[] {}),
			new ProgramDefinition("motd", "Prints the Message of the Day", new ProgramDelegate(MotdClient.Program), new string[] {}),
			new ProgramDefinition("netscan", "Scans your local network for nodes", new ProgramDelegate(NetScan.Program), new string[] {}),
			new ProgramDefinition("telnetd", "Telnet server", new ProgramDelegate(TelnetDaemon.Program), new string[] {}),
			new ProgramDefinition("c2d", "Command-and-control server", new ProgramDelegate(C2D.Program), new string[] {}),
			new ProgramDefinition("metahack", "Exploit generator and C2D client", new ProgramDelegate(MetaHack.Program), new string[] {}),
		};

		public static ProgramDefinition GetProgram(string Name)
		{
			foreach (ProgramDefinition Program in Programs)
			{
				if (Program.Name == Name)
				{
					return Program;
				}
			}
			throw new Exception("Invalid program name");
		}

		public static List<string> ListPrograms()
		{
			List<string> Items = new List<string>();
			foreach (ProgramDefinition Program in Programs)
			{
				Items.Add(Program.Name);
			}
			return Items;
		}
	}
}
