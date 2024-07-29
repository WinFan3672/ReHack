using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Data;

namespace ReHack.Node.WebServer
{
	public class WebServer : BaseNode
	{
		public string IndexFile {get; set; }
		public bool UseWhitelist {get; } = false;
		public List<string> Whitelist {get; } = new List<string>();
		public List<string> Blacklist {get; } = new List<string>();

		public WebServer(string Name, string UID, string Address, string IndexFile, string? AdminPassword=null) : base (Name, UID, Address, new User[] {
				new User("root", AdminPassword, true), new User("w3", null, false),
			})
		{
			this.IndexFile = IndexFile;
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("http"));
		}

		public string Render(BaseNode Client)
		{
			if (this.UseWhitelist && !this.Whitelist.Contains(Client.UID))
			{
				return "<Webpage><Head><Title>Error 403</Title></Head><Body><Text>This host is not on the whitelist.</Text></Body></Webpage>";
			}
			
			if (this.Blacklist.Contains(Client.UID))
			{
				return "<Webpage><Head><Title>Error 403</Title></Head><Body><Text>This host has been blacklisted by the webmaster.</Text></Body></Webpage>";
			}

			string Path = $"Web.{this.IndexFile}.xml";
			return FileUtils.GetFileContents(Path);
		}
	}
}
