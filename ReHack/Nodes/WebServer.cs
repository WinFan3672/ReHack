using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.WebRendering;
using ReHack.Networks;
using ReHack.Exceptions;

namespace ReHack.Node.Webserver
{
	public class WebServer : BaseNode
	{
		public string IndexFolder {get; set; }
		public bool UseWhitelist {get; } = false;
		public List<string> Whitelist {get; } = new List<string>();
		public List<string> Blacklist {get; } = new List<string>();

		public WebServer(string Name, string UID, string Address, AreaNetwork? Network, string IndexFolder, string? AdminPassword=null) : base (Name, UID, Address, new User[] {
				new User("root", AdminPassword, true), new User("w3", null, false),
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
			WebRender.Render("<Webpage><Head><Title>Error 403</Title></Head><Body><Text>Access to this website is denied.</Text></Body></Webpage>");
		}

		public virtual void Render(BaseNode Client, string Resource="/")
		{
			if (!this.CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}

			string Path;

			if (Resource == "/")
			{
				Path = $"Web.{this.IndexFolder}.index.xml";
			}
			else
			{
				Path = $"Web.{this.IndexFolder}.{Resource.TrimStart('/')}.xml";
			}

			WebRender.Render(FileUtils.GetFileContents(Path) ?? throw new ErrorMessageException("Invalid resource"));
		}
	}
}
