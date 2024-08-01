using ReHack.Node;
using ReHack.Node.Player;
using ReHack.Node.Mail;
using ReHack.Node.Webserver;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;
using ReHack.Node.MailSignup;
using ReHack.Node.FTP;
using ReHack.Filesystem;

namespace ReHack.Data.Nodes
{
	public static class NodeData
	{
		public static void Init(PlayerNode Player)
		{
			BaseNode TestNode = GameData.AddNode(new BaseNode("Test", "test", "test.com", new User[] { new User("root", UserUtils.PickPassword(), true) }));
			TestNode.Ports.Add(GameData.GetPort("ssh"));
			TestNode.Ports.Add(GameData.GetPort("telnet"));

			Package[] AptRepoPackages = {
				new Package("ping", new string[] {}),
				new Package("curl", new string[] {}),
			};

			Package[] ReHackPackages = {
				new Package("nmap", new string[] {}),
				new Package("hydra", new string[] {}),
				new Package("telnetpwn", new string[] {}),
			};

			var AptRepo = GameData.AddNode(new PackageRepo("Debian Official Packages", "debian-pkg", "pkg.debian.org", AptRepoPackages, null));
			var ReHackRepo = GameData.AddNode(new PackageRepo("ReHack Official Packages", "rehack-pkg", "pkg.rehack.org", ReHackPackages, null));

			WebServer TestWeb = GameData.AddNode(new WebServer("Test Page", "test-web", "www.test.com", "Test")) as WebServer ?? throw new ArgumentNullException();
			WebServer ReHackWeb = GameData.AddNode(new WebServer("ReHack", "rehack-web", "rehack.org", "ReHack")) as WebServer ?? throw new ArgumentNullException();
			WebServer Example = GameData.AddNode(new WebServer("Example Domain", "example", "example.com", "Example")) as WebServer ?? throw new ArgumentNullException();
			Example.Blacklist.Add(TestNode.UID);

			MailServer JMail = GameData.AddNode(new MailServer("JMail", "jmail", "jmail.com", null, "JMail")) as MailServer;

			MailSignupService JMailSignup = GameData.AddNode(new MailSignupService("JMail Signup", "jmail-su", "signup.jmail.com", "jmail")) as MailSignupService;

			VirtualDirectory DebianFiles = new VirtualDirectory("ftp", new VirtualFile[]{
					new VirtualFile("Debian-5.0.5.iso", ""),
					}, new VirtualDirectory[]{
					new VirtualDirectory("Test", new VirtualFile[]{}, new VirtualDirectory[]{}),
					});
			FTPServer DebianFTP = GameData.AddNode(new FTPServer("Debian FTP", "debianftp", "ftp.debian.org", DebianFiles)) as FTPServer;
		}
	}
}
