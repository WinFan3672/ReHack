using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.WebRendering;
using ReHack.Networks;

namespace ReHack.Node.Webserver
{
	public class WebServer : BaseNode
	{
		public string IndexFolder {get; set; }
		public bool UseWhitelist {get; } = false;
		public List<string> Whitelist {get; } = new List<string>();
		public List<string> Blacklist {get; } = new List<string>();

		public WebServer(string Name, string UID, string Address, AreaNetwork? Network, string IndexFolder, string? AdminPassword=null) : base (Name, UID, Address, new User[] {
				new User("root", AdminPassword, true, false), new User("w3", null, false, false),
				}, Network)
		{
			this.IndexFolder = IndexFolder;
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("http"));
		}

		public bool CheckAccessControl(BaseNode Client)
		{
			/// <summary>
			/// Checks if a node is allowed based on the whitelist and blacklist.
			/// </summary

			if (this.Blacklist.Contains(Client.UID))
			{
				// Client is blacklisted
				return false;
			}
			if (this.UseWhitelist)
			{
				// Check if whitelisted
				return this.Whitelist.Contains(Client.UID);
			}
			// All checks passed
			return true;
		}

		public virtual void RenderAccessDenied()
		{
			WebRender.Render(WebRender.GenerateBasicWebpage("Error 403", "Access to this website is denied.", $"Apache Web Server"));
		}

		public virtual void Render(BaseNode Client)
		{
			if (!this.CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}

			string Path = $"Web.{this.IndexFolder}.index.xml";
			WebRender.Render(FileUtils.GetFileContents(Path) ?? throw new FileNotFoundException());
		}
	}
}
