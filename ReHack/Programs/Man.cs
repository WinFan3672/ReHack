using System.Reflection;
using ReHack.Node;
using ReHack.Data;
using ReHack.BaseMethods;
using System.Xml;
using Spectre.Console;

namespace ReHack.Programs.Man
{
	public static class ManClient
	{
		public static void ReadManpage(string Manpage)
		{
			Console.Clear();
			XmlDocument Doc = new XmlDocument();
			string Raw = FileUtils.GetFileContents($"Man.{Manpage}.xml");

			Doc.LoadXml(Raw);
			XmlElement Root = Doc.DocumentElement;

			string Title = Doc.SelectSingleNode("//Title").InnerText;
			PrintUtils.PrintCentered(Title, Console.WindowWidth, '─', '│');

			XmlNodeList Sections = Doc.SelectNodes("//Section");
			
			int Lines = 2;
			foreach (XmlNode Section in Sections)
			{
				Console.WriteLine(Section.Attributes["title"]?.Value.ToUpper() ?? "NONE");
				Lines++;
				foreach(var Line in Section.InnerText.Split("\n"))
				{
					Console.WriteLine($"\t{Line}");
					Lines++;
				}
			}
			for (int i = 0; i < Console.WindowHeight - Lines - 1; i++)
			{
				Console.WriteLine();
			}
			
			string Epilog;
			try
			{
				Epilog = Doc.SelectSingleNode("//Epilog").InnerText;
			}
			catch (NullReferenceException)
			{
				Epilog = "(c) 2010 Debian, ReHack";
			}
			PrintUtils.PrintCentered(Epilog, Console.WindowWidth, '─', '│');
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
