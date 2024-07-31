using ReHack.Node;
using ReHack.Node.WebServer;
using ReHack.BaseMethods;
using System.Xml;
using Spectre.Console;

namespace ReHack.Programs.W3
{
	public static class W3
	{
		public static void RenderNode(XmlNode Node, string Title)
		{
			if (Node.Name == "Title")
			{
				PrintUtils.PrintCentered(Title, Console.WindowWidth, '─', '─');
			}
			else if (Node.Name == "Menu")
			{
				List<string> MenuTexts = new List<string>();
				foreach(XmlNode Child in Node.ChildNodes)
				{
					if (Child.Name == "Link")
					{
						MenuTexts.Add("[" + PrintUtils.FormatUnderlined(Child.InnerText) + "]");
					}
					else
					{
						AnsiConsole.MarkupLine("[bold red]error[/]: [yellow]Menu[/] only accepts [blue]Link[/] tags");
					}
				}
				List<Text> columns = new List<Text>() {};
				foreach(string MenuText in MenuTexts)
				{
					columns.Add(new Text(MenuText));
				}
				AnsiConsole.Write(new Columns(columns));
			}
			else if (Node.Name == "Text")
			{
				XmlAttribute? Style = Node.Attributes["style"];
				if (Style == null)
				{
					AnsiConsole.MarkupLine(Node.InnerText);
				}
				else if (Style.Value == "Centered")
				{
					AnsiConsole.MarkupLine(PrintUtils.FormatCentered(Node.InnerText, Console.WindowWidth));
				}
				else
				{
					AnsiConsole.MarkupLine($"[bold red]error[/]: Text style [yellow]{Style.Value}[/] invalid"); 
				}
			}
			else if (Node.Name == "Footer")
			{
				PrintUtils.PrintCentered(Node.InnerText, Console.WindowWidth, '─', '─');
			}
			else if (Node.Name == "List")
			{
				foreach (XmlNode Child in Node.ChildNodes)
				{
					Console.WriteLine($"* {Child.InnerText}");
				}
			}
			else if (Node.Name == "Break")
			{
				XmlAttribute? Style = Node.Attributes["style"];
				if (Style == null)
				{
					Console.WriteLine();
				}
				else if (Style.Value == "Divider")
				{
					Console.WriteLine(new string('─', Console.WindowWidth));
				}
				else
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: [yellow]Break[/] tag has invalid [blue]style[/] attribute");
				}
			}
			else if (Node.Name == "Link")
			{
				XmlAttribute? LinkKind = Node.Attributes["kind"];
				XmlAttribute? LinkRef = Node.Attributes["ref"];
				XmlAttribute? LinkStyle = Node.Attributes["style"];
				AnsiConsole.MarkupLine("[bold red]error[/]: [yellow]Link[/] tags not implemented in [blue]W3[/]");
			}
			else if (Node.Name == "Box")
			{

				Console.WriteLine("┌" + new string('─', Console.WindowWidth - 2) + "┐");
				foreach(XmlNode Child in Node.ChildNodes)
				{
					if (Child.Name == "Text")
					{
						Console.WriteLine("│" + PrintUtils.FormatCentered(Child.InnerText, Console.WindowWidth - 8).TrimEnd('\r', '\n') + "│");
					}
					else
					{
						AnsiConsole.MarkupLine("[bold red]error[/]: [yellow]Box[/] only accepts [blue]Text[/] objects");
					}
				}
				Console.WriteLine("└" + new string('─', Console.WindowWidth - 2) + "┘");
			}
			else if (Node.Name == "Heading")
			{
				AnsiConsole.MarkupLine($"[bold]{Node.InnerText}[/]");
			}
			else if (Node.Name == "Command")
			{
				XmlAttribute? Command = Node.Attributes["cmd"];
				if (Command == null)
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: [yellow]Command[/] tag requires a [blue]cmd[/] attribute");
				}
				else if (Command.Value == "Clear")
				{
					Console.Clear();
				}
				else
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: [yellow]Command[/] tag has invalid [blue]cmd[/] attribute");
				}
			}
			else
			{
				AnsiConsole.MarkupLine($"[bold red]error[/]: Invalid tag [yellow]{Node.Name}[/]");
			}
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 0)
			{
				Console.WriteLine("usage: w3 [hostname]");
				return false;
			}
			if (!NodeUtils.CheckNodeByAddress(Args[0]))
			{
				Console.WriteLine("error: Invalid hostname");
				return false;
			}
			if (!NodeUtils.CheckPort(NodeUtils.GetNodeByAddress(Args[0]), "http"))
			{
				Console.WriteLine("error: Connection refused");
				return false;
			}

			WebServer Server = NodeUtils.GetNodeByAddress(Args[0]) as WebServer;
			XmlDocument Doc = new XmlDocument();
			Doc.LoadXml(Server.Render(Client, "/"));
			string? Title = Doc.SelectSingleNode("//Head/Title").InnerText ?? "Untitled";
			foreach(XmlNode Node in Doc.SelectSingleNode("//Body").ChildNodes)
			{
				RenderNode(Node, Title);
			}
			return true;
		}
	}
}
