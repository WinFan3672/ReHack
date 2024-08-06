using ReHack.BaseMethods;
using ReHack.WebRendering;
using ReHack.Networks;

namespace ReHack.Node.Webserver
{
	/// <summary>Web server.</summary>
	public class WebServer : BaseNode
	{
		/// <summary>The folder containing the index.xml</summary>
		public string IndexFolder {get; set; }
		/// <summary>Whether or not to use the whitelist.</summary>
		public bool UseWhitelist {get; set; } = false;
		/// <summary>Access whitelist - contains UIDs not addresses</summary>
		public List<string> Whitelist {get; set; } = new List<string>();
		/// <summary>Access blacklist - contains UIDs not addresses</summary>
		public List<string> Blacklist {get; set; } = new List<string>();

		///
		public WebServer(string Name, string UID, string Address, AreaNetwork? Network, string IndexFolder, string? AdminPassword=null) : base (Name, UID, Address, new User[] {
				new User("root", AdminPassword, true, false), new User("w3", null, false, false),
				}, Network)
		{
			this.IndexFolder = IndexFolder;
			AddPort("ssh");
			AddPort("ftp");
			AddPort("http");
		}

		/// <summary>
		/// Checks if a node is allowed based on the whitelist and blacklist.
		/// </summary>
		public bool CheckAccessControl(BaseNode Client)
		{

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

		/// <summary>Renders an access denied page.</summary>
		public virtual void RenderAccessDenied()
		{
			WebRender.Render(WebRender.GenerateBasicWebpage("Error 403", "Access to this website is denied.", $"Apache Web Server"));
		}

		/// <summary>Renders content.</summary>
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
