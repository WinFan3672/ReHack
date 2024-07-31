using ReHack.Node;
using ReHack.Node.WebServer;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;

namespace ReHack.Data.Nodes
{
	public static class NodeData
	{
		public static void Init()
		{
			BaseNode TestNode = GameData.AddNode(new BaseNode("Test", "test", "test.com", new User[] { new User("root", UserUtils.PickPassword(), true) }));
			TestNode.Ports.Add(GameData.GetPort("ssh"));

			Package[] AptRepoPackages = {
				new Package("ping", new string[] {}),
				new Package("curl", new string[] {}),
			};

			Package[] ReHackPackages = {
				new Package("nmap", new string[] {}),
				new Package("hydra", new string[] {}),
			};

			var AptRepo = GameData.AddNode(new PackageRepo("Debian Official Packages", "debian-pkg", "pkg.debian.org", AptRepoPackages, null));
			var ReHackRepo = GameData.AddNode(new PackageRepo("ReHack Official Packages", "rehack-pkg", "pkg.rehack.org", ReHackPackages, null));

			WebServer TestWeb = GameData.AddNode(new WebServer("Test Page", "test-web", "www.test.com", "Test")) as WebServer ?? throw new ArgumentNullException();
			WebServer ReHackWeb = GameData.AddNode(new WebServer("ReHack", "rehack-web", "rehack.org", "ReHack")) as WebServer ?? throw new ArgumentNullException();
			WebServer Example = GameData.AddNode(new WebServer("Example Domain", "example", "example.com", "Example")) as WebServer ?? throw new ArgumentNullException();
		}
	}
}
