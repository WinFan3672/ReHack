using ReHack.BaseMethods;
using ReHack.Programs.SSHClient;
using ReHack.Programs.Help;
using ReHack.Programs.Man;
using ReHack.Programs.Ping;

namespace ReHack.Data.Programs
{
	public class ProgramData
	{
		public static List<ProgramDefinition> Programs = new List<ProgramDefinition> {
			new ProgramDefinition("ssh", "SSH Client", new ProgramDelegate(SSHClient.Program)),
			new ProgramDefinition("help", "Lists available programs", new ProgramDelegate(HelpClient.Program)),
			new ProgramDefinition("man", "Manual viewer", new ProgramDelegate(ManClient.Program)),
			new ProgramDefinition("lsman", "List available manuals", new ProgramDelegate(ManClient.ListProgram)),
			new ProgramDefinition("ping", "Checks if hosts are up", new ProgramDelegate(PingClient.Program)),
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
