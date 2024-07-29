using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Data;

namespace ReHack.Node.WebServer
{
	public class WebServer : BaseNode
	{
		public string IndexFile {get; set; }

		public WebServer(string Name, string UID, string Address, string IndexFile, string? AdminPassword=null) : base (Name, UID, Address, new User[] {
				new User("root", AdminPassword, true), new User("w3", null, false),
			})
		{
			this.IndexFile = IndexFile;
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("http"));
		}

		public string Render()
		{
			string Path = $"Web.{this.IndexFile}.xml";
			return FileUtils.GetFileContents(Path);
		}
	}
}
