using ReHack.Node;
using ReHack.BaseMethods;
using System.Xml;

namespace ReHack.Programs.Man
{
	/// <summary>Reads manpages.</summary>
	public static class ManClient
	{
		/// <summary>Reads a manpage.</summary>
		public static void ReadManpage(string Manpage)
		{
			Console.Clear();
			XmlDocument Doc = new XmlDocument();
			string Raw = FileUtils.GetFileContents($"Man.{Manpage}.xml") ?? throw new FileNotFoundException();

			Doc.LoadXml(Raw);
			XmlElement Root = Doc.DocumentElement ?? throw new XmlException("No root element found");
			
			XmlNode TitleNode = Doc.SelectSingleNode("//Title") ?? throw new XmlException("No title element");
			string Title = TitleNode.InnerText;
			PrintUtils.PrintCentered(Title, Console.WindowWidth, '─', '│');

			XmlNodeList Sections = Doc.SelectNodes("//Section") ?? throw new XmlException("No sections found");
			
			int Lines = 2;
			XmlAttributeCollection Attributes;
			foreach (XmlNode Section in Sections)
			{
				Attributes = Section.Attributes ?? throw new XmlException("No section title found");
				Console.WriteLine(Attributes["title"]?.Value.ToUpper() ?? "NONE");
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
			XmlNode EpilogRaw;
			try
			{
				EpilogRaw = Doc.SelectSingleNode("//Epilog") ?? throw new XmlException();
				Epilog = EpilogRaw.InnerText;
			}
			catch (XmlException)
			{
				Epilog = "(c) 2010 Debian, ReHack";
			}
			PrintUtils.PrintCentered(Epilog, Console.WindowWidth, '─', '│');
		}
		/// <summary>The lsman program.</summary>
		public static bool ListProgram(string[] Args, BaseNode Client, User RunningUser)
		{
			foreach (string Manpage in Client.ListManpages())
			{
				Console.WriteLine(Manpage);
			}
			return true;
		}
		/// <summary>Program function.</summary>
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
