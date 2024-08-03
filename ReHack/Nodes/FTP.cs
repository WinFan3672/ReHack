using ReHack.Node.Webserver;
using ReHack.BaseMethods;
using ReHack.Filesystem;
using ReHack.WebRendering;
using ReHack.Data;
using ReHack.Networks;

namespace ReHack.Node.FTP
{
	public class FTPServer : WebServer 
	{
		public VirtualDirectory Folder {get; set; }
		public bool Anonymous {get; set; }
		public FTPServer(string Name, string UID, string Address, VirtualDirectory Folder, AreaNetwork? Network, bool Anonymous=true, string? UserPassword=null, string? AdminPassword=null) : base(Name, UID, Address, Network, "This variable isn't used", AdminPassword)
		{
			this.Anonymous = Anonymous;
			if (Folder.Name != "ftp")
			{
				throw new ArgumentException("Invalid folder name");
			}
			this.Folder = Folder;
			this.Root.GetDirectory("/var");

			Ports.Add(GameData.GetPort("ftp"));
			Ports.Add(GameData.GetPort("telnet"));

			User FtpUser = new User("ftpuser", UserPassword, false, false);
			this.Users = MiscUtils.AddItemToArray(this.Users, FtpUser);
			InitUser(FtpUser);
		}

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

			WebRender.Render("<Website><Head><Title>Apache FTP Server</Title></Head><Body><Title /><Text>To connect to this server, use an FTP client.</Text><Footer>(c) 2010 Apache Software Foundation</Footer></Body></Website>");
		}
	}
}
