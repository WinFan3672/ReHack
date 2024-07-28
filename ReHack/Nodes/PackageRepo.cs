using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Data.Programs;

namespace ReHack.Node.PackageRepo
{
	public class PackageRepo : BaseNode 
	{
		public Package[] Packages {get; set; }

		public PackageRepo(string Name, string UID, string Address, Package[] Packages, string? AdminPassword) : base(Name, UID, Address, new User[] { new User("root", null, true), new User("apt",  AdminPassword, false)})
		{
			this.Packages = Packages;
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("ftp"));
		}
	}

	public class Package
	{
		public string Name {get; set; }
		public string[] Dependencies {get; set; }

		public Package(string Name, string[] Dependencies)
		{
			this.Name = Name;
			this.Dependencies = Dependencies;
		}

		public void Install(BaseNode Client)
		{
			Client.AddProgram(this.Name);
		}
	}
	
}
