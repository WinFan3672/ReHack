using System.Reflection;
using ReHack.Node;
using ReHack.Data;
using ReHack.BaseMethods;
using System.Xml;

namespace ReHack.Programs.Man
{
	public static class ManClient
	{
		public static void ReadManpage(string Manpage)
		{
			XmlDocument Doc = new XmlDocument();
			string Raw = FileUtils.GetFileContents($"Man/{Manpage}.xml");
		}
		public static bool ListProgram(string[] Args, BaseNode Client, User RunningUser)
		{
			foreach (string Manpage in Client.ListManpages())
			{
				Console.WriteLine(Manpage);
			}
			return true;
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 1)
			{
				if (Client.ListManpages().Contains(Args[0]))
				{
					ReadManpage(Args[0]);
					return true;
				}
				else
				{
					Console.WriteLine($"No manual entry for {Args[0]}");
					return false;
				}
			}
			else
			{
				Console.WriteLine("What manual page do you want?");
				Console.WriteLine("For example, try 'man man'.");
				return false;
			}
		}
	}
}
