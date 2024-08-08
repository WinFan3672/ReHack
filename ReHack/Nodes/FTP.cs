using ReHack.Node.Webserver;
using ReHack.BaseMethods;
using ReHack.Filesystem;
using ReHack.WebRendering;
using ReHack.Data;
using ReHack.Networks;

namespace ReHack.Node.FTP
{
	/// <summary>FTP server. The web server part is to render a 'You need an FTP client' message.</summary>
	public class FTPServer : WebServer 
	{
		/// <summary>Where the FTP data is held.</summary>
		public VirtualDirectory Folder {get; set; }
		/// <summary>Whether anonymous login is allowed</summary>
		public bool Anonymous {get; set; }

		/// <summary>Constructor</summary>
		public FTPServer(string Name, string UID, string Address, AreaNetwork? Network, bool Anonymous=true, string? UserPassword=null, string? AdminPassword=null) : base(Name, UID, Address, Network, "This variable isn't used", AdminPassword)
		{
			this.Anonymous = Anonymous;
			Folder = Root.GetDirectory("/srv/ftp");

			Ports.Add(GameData.GetPort("ftp"));
			Ports.Add(GameData.GetPort("telnet"));

			User FtpUser = new User("ftpuser", UserPassword, false, false);
			this.Users = MiscUtils.AddItemToArray(this.Users, FtpUser);
			InitUser(FtpUser);
		}

		/// <summary>Renders a message saying you need to use an FTP client.</summary>
		public override void Render(BaseNode Client)
		{
			if (!this.CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}

			if (!Anonymous && !Authenticate(GetUser("ftpuser")))
			{
				RenderAccessDenied();
				return;
			}

			WebRender.Render(WebRender.GenerateBasicWebpage("Apache FTP Server", "To connect to this server, use an FTP Client.", "(c) 2010 Apache Software Foundation"));
		}
	}
}
