using ReHack.Node;
using ReHack.Node.Webserver;
using ReHack.BaseMethods;
using ReHack.Filesystem;
using ReHack.WebRendering;
using System.Xml;

namespace ReHack.Node.FTP
{
	public class FTPServer : WebServer 
	{
		public VirtualDirectory Folder {get; set; }
		public FTPServer(string Name, string UID, string Address, VirtualDirectory Folder, string? AdminPassword=null) : base(Name, UID, Address, "This variable isn't used", AdminPassword)
		{
			if (Folder.Name != "ftp")
			{
				throw new ArgumentException("Invalid folder name");
			}
			this.Folder = Folder;
			this.Root.GetDirectory("/var");
		}

		public override void Render(BaseNode Client, string Resource="/")
		{
			if (!this.CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}

			XmlDocument Doc = new XmlDocument();

			XmlElement Root = Doc.CreateElement("Website");

			XmlElement Head = Doc.CreateElement("Head");
			XmlElement Title = Doc.CreateElement("Title");
			Title.InnerText = $"Index of {Resource}";

			Root.AppendChild(Head);
			Head.AppendChild(Title);

			XmlElement Body = Doc.CreateElement("Body");
			Body.AppendChild(Doc.CreateElement("Title"));

			XmlElement Child;
			XmlAttribute Style = Doc.CreateAttribute("style");
			
			foreach (VirtualDirectory Dir in Folder.SubDirectories)
			{
				Child = Doc.CreateElement("Text");
				Child.InnerText = $"* [aqua underline]{Dir.Name}/[/]";
				Body.AppendChild(Child);
			}
			
			foreach(VirtualFile File in Folder.Files)
			{
				Child = Doc.CreateElement("Text");
				Child.InnerText = $"* [blue underline]{File.Name}[/]";
				Body.AppendChild(Child);
			}

			XmlElement Footer = Doc.CreateElement("Footer");
			Footer.InnerText = "Apache FTP Server (c) 2010 Apache Software Foundation";

			Body.AppendChild(Footer);

			Root.AppendChild(Body);
			Doc.AppendChild(Root);

			WebRender.Render(Doc.OuterXml);
		}
	}
}
