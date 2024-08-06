using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Data.Programs;

namespace ReHack.Programs.Help {
	/// <summary>Lists installed programs.</summary>
	public static class HelpClient
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
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
