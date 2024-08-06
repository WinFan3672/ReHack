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
using ReHack.Node.News;
using ReHack.Node.Wiki;
using ReHack.Node.Bank;

namespace ReHack.Data.Nodes
{
	/// <summary>Node data.</summary>
	public static class NodeData
	{
		/// <summary>Creates the in-game nodes.</summary>
		public static void Init(PlayerNode Player)
		{
			BaseNode TestNode = GameData.AddNode(new BaseNode("Test", "test", "test.com", new User[] { 
						new User("root", UserUtils.PickPassword(), true, false),
						new User("john", UserUtils.PickPassword(), false, true),
			}, new AreaNetwork()));
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
				new Package("c2d", new string[] {}),
				new Package("metahack", new string[] {"c2d", "telnetpwn"}),
			};

			PackageRepo? AptRepo = GameData.AddNode(new PackageRepo("Debian Official Packages", "debian-pkg", "pkg.debian.org", AptRepoPackages, null, null)) as PackageRepo;
			PackageRepo? ReHackRepo = GameData.AddNode(new PackageRepo("ReHack Official Packages", "rehack-pkg", "pkg.rehack.org", ReHackPackages, null, null)) as PackageRepo;

			WebServer TestWeb = GameData.AddNode(new WebServer("Test Page", "test-web", "www.test.com", null, "Test")) as WebServer ?? throw new ArgumentNullException();
			WebServer ReHackWeb = GameData.AddNode(new WebServer("ReHack", "rehack-web", "rehack.org", null, "ReHack")) as WebServer ?? throw new ArgumentNullException();
			WebServer Example = GameData.AddNode(new WebServer("Example Domain", "example", "example.com", null, "Example")) as WebServer ?? throw new ArgumentNullException();
			Example.Blacklist.Add(TestNode.UID);

			BankNode ReHackBank = GameData.AddNode(new BankNode("ReHack Bank", "rehack-bank", "bank.rehack.org", null, null, int.MaxValue)) as BankNode ?? throw new ArgumentException();

			MailServer JMail = GameData.AddNode(new MailServer("JMail", "jmail", "jmail.com", null, "JMail")) as MailServer ?? throw new ArgumentException();
			JMail.AllowLookup = false;

			MailSignupService? JMailSignup = GameData.AddNode(new MailSignupService("JMail Signup", "jmail-su", "signup.jmail.com", null, "jmail")) as MailSignupService;

			VirtualDirectory DebianFiles = new VirtualDirectory("ftp", new VirtualFile[]{
					new VirtualFile("README", "This directory contains the ISO files for the latest Debian release."),
					new VirtualFile("Debian-5.0.1-amd64.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.2-amd64.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.3-amd64.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.4-amd64.iso", FileUtils.GenerateBytes()),
					new VirtualFile("Debian-5.0.5-amd64.iso", FileUtils.GenerateBytes()),
					}, new VirtualDirectory[]{
					new VirtualDirectory("Archives", new VirtualFile[]{
							new VirtualFile("README", "This directory contains old Debian release ISOs."),
							}, new VirtualDirectory[] {
							new VirtualDirectory("4.0", new VirtualFile[]{
									new VirtualFile("debian-4.0r1-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r2-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r3-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r4-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r5-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r6-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r7-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r8-amd64.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-4.0r9-amd64.iso", FileUtils.GenerateBytes()),
									}, new VirtualDirectory[] {}),
							new VirtualDirectory("3.0", new VirtualFile[] {
									new VirtualFile("debian-3.0r1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.0r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.0r3-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.0r4-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.0r5-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.0r6-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r3-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r4-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r5-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r6-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r7-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-3.1r8-i386.iso", FileUtils.GenerateBytes()),
									}, new VirtualDirectory[] {}),
							new VirtualDirectory("2.0", new VirtualFile[] {
									new VirtualFile("debian-2.0r1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.0r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.0r3-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.0r4-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.0r5-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.1r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.1r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.1r3-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.1r4-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.1r5-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r3-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r4-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r5-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r6-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-2.2r7-i386.iso", FileUtils.GenerateBytes()),
									}, new VirtualDirectory[] {}),
							new VirtualDirectory("1.0", new VirtualFile[] {
									new VirtualFile("debian-1.1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.0-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1r1-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1r2-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1r3-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1r4-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1r5-i386.iso", FileUtils.GenerateBytes()),
									new VirtualFile("debian-1.3.1r6-i386.iso", FileUtils.GenerateBytes()),
									}, new VirtualDirectory[] {}),
							}),
					});

			FTPServer? DebianFTP = GameData.AddNode(new FTPServer("Debian FTP", "debianftp", "ftp.debian.org", DebianFiles, null)) as FTPServer;

			NewsServer MHT = GameData.AddNode(new NewsServer("MHT", "mht", "mht.com", null)) as NewsServer ?? throw new ArgumentException();
			NewsUtils.AddArticleFromFile("office2010");

			MailServer EnWired = GameData.AddNode(new MailServer("EnWired", "enwired", "enwired.com", null, null, "EnWired")) as MailServer ?? throw new ArgumentException();
			EnWired.CreateMailAccount("sales", null);
			EnWired.CreateMailAccount("elliot.marksman", null);

			WikiServer RHWiki = GameData.AddNode(new WikiServer("ReHack Wiki", "rhwiki", "wiki.rehack.org", null, null)) as WikiServer ?? throw new ArgumentException();
			WikiCategory RHWiki_Hacking = RHWiki.RootPage.AddCategory("Hacking");

			WebServer Goph = GameData.AddNode(new WebServer("Gop's Guide To Hackery :: Homepage", "gophweb", "goph.org", null, "Goph")) as WebServer ?? throw new ArgumentException();

			WebServer ReHackDL = GameData.AddNode(new WebServer("ReHack Client", "rehackdl", "dl.rehack.org", null, "ReHackDL")) as WebServer ?? throw new ArgumentException();
			ReHackDL.UseWhitelist = true;
		}
	}
}
