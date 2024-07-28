using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Data.Programs;

namespace ReHack.Programs.Help {
	public static class HelpClient
	{
		public static bool Program(string[] Args, BaseNode Client)
		{
			PrintUtils.Divider();
			Console.WriteLine("Program List");
			PrintUtils.Divider();
			foreach(string Item in Client.ListPrograms())
			{
				ProgramDefinition Prog = ProgramData.GetProgram(Item);
				Console.WriteLine($"{Prog.Name} - {Prog.Description}");
			}
			PrintUtils.Divider();
			return true;
		}
	}
}
