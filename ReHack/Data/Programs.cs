using ReHack.BaseMethods;
using ReHack.Programs.SSHClient;
using ReHack.Programs.Help;
using ReHack.Programs.Man;
using ReHack.Programs.Ping;
using ReHack.Programs.Apt;
using ReHack.Programs.Debug;

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
			new ProgramDefinition("apt", "Package Manager", new ProgramDelegate(Apt.Program), new string[] {} ),
			new ProgramDefinition("debug", "Debugging tools", new ProgramDelegate(DebugClient.Program), new string[] {} ),
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
