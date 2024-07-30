using ReHack.Node;
using ReHack.Node.WebServer;
using ReHack.BaseMethods;
using System.Xml;

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
				}
				PrintUtils.PrintCentered(string.Join(" ", MenuTexts), Console.WindowWidth);
			}
			else if (Node.Name == "Text")
			{
				XmlAttribute? Style = Node.Attributes["style"];
				if (Style == null)
				{
					Console.WriteLine(Node.InnerText);
				}
				else if (Style.Value == "Centered")
				{
					PrintUtils.PrintCentered(Node.InnerText, Console.WindowWidth);
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
			}
			else if (Node.Name == "Link")
			{
				XmlAttribute? LinkKind = Node.Attributes["kind"];
				XmlAttribute? LinkRef = Node.Attributes["ref"];
				XmlAttribute? LinkStyle = Node.Attributes["style"];
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
				}
				Console.WriteLine("└" + new string('─', Console.WindowWidth - 2) + "┘");
			}
			else
			{
				Console.WriteLine(Node.OuterXml);
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
