using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;
using System.Xml;

namespace ReHack.Programs.Welcome
{
	public static class WelcomeApp
	{
		public static void ReadXml(string Page)
		{
			XmlDocument Doc = new XmlDocument();
			Doc.LoadXml(FileUtils.GetFileContents($"Welcome.{Page}.xml") ?? throw new FileNotFoundException());
			XmlElement Root = Doc.DocumentElement ?? throw new XmlException("Document has no root tag");
			XmlNode TitleRaw = Doc.SelectSingleNode("//Title") ?? throw new XmlException("No title");
			string Title = TitleRaw.InnerText;
			Console.Clear();
			AnsiConsole.MarkupLine($"[bold]{Title}[/]");
			foreach(XmlNode Node in Root.ChildNodes)
			{
				if (Node.Name == "Heading")
				{
					Console.WriteLine();
					AnsiConsole.MarkupLine($"[bold]{Node.InnerText}[/]");
				}
				else if (Node.Name == "Text")
				{
					AnsiConsole.MarkupLine(Node.InnerText);
				}
			}
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			Console.Clear();
			string Option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select Section To Read").AddChoices(new[] {"The Basics", "Internet Resources"}));
			switch (Option)
			{
				case "The Basics":
					ReadXml("Basics");
					break;
				case "Internet Resources":
					ReadXml("Internet");
					break;
			}
			
			return true;
		}
	}
}
