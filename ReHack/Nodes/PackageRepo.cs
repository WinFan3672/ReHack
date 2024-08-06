using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Networks;

namespace ReHack.Node.PackageRepo
{
	/// <summary>A package repo holds packages.</summary>
	public class PackageRepo : BaseNode 
	{
		///
		public Package[] Packages {get; set; }

		///
		public PackageRepo(string Name, string UID, string Address, Package[] Packages, AreaNetwork? Network, string? AdminPassword) : base(Name, UID, Address, new User[] { new User("root", AdminPassword, true, false), new User("apt", null, false, false)}, Network)
		{
			this.Packages = Packages;
			this.Ports.Add(GameData.GetPort("ssh"));
		}

		/// <summary>Lists packages.</summary>
		public List<string> ListPackages()
		{
			List<string> Packages = new List<string>();
			foreach(Package Item in this.Packages)
			{
				Packages.Add(Item.Name);
			}
			return Packages;
		}

		/// <summary>Gets a package from a name.</summary>
		public Package GetPackage(string Name)
		{
			foreach (Package Item in this.Packages)
			{
				if (Name == Item.Name)
				{
					return Item;
				}
			}
			throw new Exception("Invalid package");
		}
	}

	/// <summary>A package.</summary>
	public class Package
	{
		///
		public string Name {get; set; }
		/// <summary>Packages the package needs to install.</summary>
		public string[] Dependencies {get; set; }

		///
		public Package(string Name, string[] Dependencies)
		{
			this.Name = Name;
			this.Dependencies = Dependencies;
		}

		/// <summary>Installs a package.</summary>
		public void Install(BaseNode Client)
		{
			Client.AddProgram(this.Name);
		}
	}
	
}
