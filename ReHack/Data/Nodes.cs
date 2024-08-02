using ReHack.Node;
using ReHack.Node.Player;
using ReHack.Node.Mail;
using ReHack.Node.Webserver;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;
using ReHack.Node.MailSignup;
using ReHack.Node.FTP;
using ReHack.Filesystem;
using ReHack.Networks;

namespace ReHack.Data.Nodes
{
	public static class NodeData
	{
		public static void Init(PlayerNode Player)
		{
			BaseNode TestNode = GameData.AddNode(new BaseNode("Test", "test", "test.com", new User[] { new User("root", UserUtils.PickPassword(), true) }, new AreaNetwork()));
			TestNode.Ports.Add(GameData.GetPort("ssh"));
			TestNode.Ports.Add(GameData.GetPort("telnet"));

			Package[] AptRepoPackages = {
				new Package("netscan", new string[] {}),
				new Package("telnetd", new string[] {}),
			};

			Package[] ReHackPackages = {
				new Package("nmap", new string[] {}),
				new Package("hydra", new string[] {}),
				new Package("telnetpwn", new string[] {}),
				new Package("mxlookup", new string[] {}),
			};

			PackageRepo? AptRepo = GameData.AddNode(new PackageRepo("Debian Official Packages", "debian-pkg", "pkg.debian.org", AptRepoPackages, null, null)) as PackageRepo;
			PackageRepo? ReHackRepo = GameData.AddNode(new PackageRepo("ReHack Official Packages", "rehack-pkg", "pkg.rehack.org", ReHackPackages, null, null)) as PackageRepo;

			WebServer TestWeb = GameData.AddNode(new WebServer("Test Page", "test-web", "www.test.com", null, "Test")) as WebServer ?? throw new ArgumentNullException();
			WebServer ReHackWeb = GameData.AddNode(new WebServer("ReHack", "rehack-web", "rehack.org", null, "ReHack")) as WebServer ?? throw new ArgumentNullException();
			WebServer Example = GameData.AddNode(new WebServer("Example Domain", "example", "example.com", null, "Example")) as WebServer ?? throw new ArgumentNullException();
			Example.Blacklist.Add(TestNode.UID);

			MailServer? JMail = GameData.AddNode(new MailServer("JMail", "jmail", "jmail.com", null, "JMail")) as MailServer;

			MailSignupService? JMailSignup = GameData.AddNode(new MailSignupService("JMail Signup", "jmail-su", "signup.jmail.com", null, "jmail")) as MailSignupService;

			VirtualDirectory DebianFiles = new VirtualDirectory("ftp", new VirtualFile[]{
					new VirtualFile("README", "This directory contains the ISO files for the latest Debian release."),
					new VirtualFile("Debian-5.0.1-i686.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.2-i686.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.3-i686.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.4-i686.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.5-i686.iso", FileUtils.GenerateBytes()),
					}, new VirtualDirectory[]{
						new VirtualDirectory("Archives", new VirtualFile[]{
								new VirtualFile("README", "This directory contains old Debian release ISOs."),
								}, new VirtualDirectory[] {
								new VirtualDirectory("4.0", new VirtualFile[]{
										new VirtualFile("Debian-4.0r1-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r2-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r3-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r4-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r5-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r6-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r7-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r8-i686.iso", FileUtils.GenerateBytes()),
										new VirtualFile("Debian-4.0r9-i686.iso", FileUtils.GenerateBytes()),
										}, new VirtualDirectory[] {}),
								}),
					});

			FTPServer? DebianFTP = GameData.AddNode(new FTPServer("Debian FTP", "debianftp", "ftp.debian.org", DebianFiles, null)) as FTPServer;
		}
	}
}
