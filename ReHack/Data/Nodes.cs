using ReHack.Node;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;

namespace ReHack.Data.Nodes
{
	public static class NodeData
	{
		public static void Init()
		{
			BaseNode TestNode = GameData.AddNode(new BaseNode("Test", "test", "test.com", new User[] { new User("root", "root", true) }));
			TestNode.Ports.Add(GameData.GetPort("ssh"));

			Package[] AptRepoPackages = {};
			var AptRepo = GameData.AddNode(new PackageRepo("Debian Official Packages", "debian-pkg", "pkg.debian.org", AptRepoPackages, null));
		}
	}
}
