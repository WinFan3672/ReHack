using ReHack.Node;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;
using Spectre.Console;
using ReHack.Filesystem;
using ReHack.Exceptions;

namespace ReHack.Programs.Apt
{
	/// <summary>Package manager program.</summary>
	public static class Apt
	{
		/// <summary>Returns all the packages of a package repository</summary>
		public static Package[] GetRepoPackages(string Address)
		{
			PackageRepo Repo = NodeUtils.GetNodeByAddress(Address) as PackageRepo ?? throw new ArgumentException("Invalid address");
			return Repo.Packages;
		}
		/// <summary>Returns all packages a node can install.</summary>
		public static List<Package> GetPackages(BaseNode Client)
		{
			List<Package> Packages = new List<Package>();
			VirtualFile RepoFile = Client.Root.GetFile("/etc/apt/sources.list") ?? throw new ArgumentException("Invalid repo address");
			foreach(string Repo in RepoFile.Content.Split("\n"))
			{
				foreach(Package Item in GetRepoPackages(Repo))
				{
					Packages.Add(Item);
				}
			}
			return Packages;
		}

		/// <summary>Returns a package from a list of packages by name</summary>
		public static Package? GetPackage(List<Package> Packages, string Name)
		{
			foreach(Package Item in Packages)
			{
				if (Item.Name == Name)
				{
					return Item;
				}
			}
			return null;
		}

		/// <summary>Turns a list of packages into a list of package names.</summary>
		public static List<string> ListPackages(List<Package> Packages)
		{
			List<string> Names = new List<string>();
			foreach(Package Item in Packages)
			{
				Names.Add(Item.Name);
			}
			return Names;
		}
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Contains("install") && Args.Length == 2)
			{
				Client.CheckPerms(RunningUser);
				string Program = Args.FirstOrDefault(Item => Item != "install") ?? throw new ArgumentException();
				if (Client.InstalledPrograms.Contains(Program))
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Package is already installed");
					return false;
				}

				Package PackageToInstall = GetPackage(GetPackages(Client), Program) ?? throw new ErrorMessageException("Invalid package");

				PackageToInstall.Install(Client);
				Console.WriteLine($"Package '{PackageToInstall.Name}' successfully installed.");

				foreach(string Dep in PackageToInstall.Dependencies)
				{
					if (!Client.ListPrograms().Contains(Dep))
					{
						Apt.Program(new [] {"install", Dep}, Client, RunningUser);
					}
				}

				return true;
			}
			else if (Args.Contains("remove") && Args.Length == 2)
			{
				string Program = Args.FirstOrDefault(Item => Item != "remove") ?? throw new ArgumentException();
				if (GetPackage(GetPackages(Client), Program) == null)
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Invalid package");
					return false;
				}
				if (!Client.InstalledPrograms.Contains(Program))
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Package not installed.");
				}
				Client.RemoveProgram(Program);
				Console.WriteLine("Successfully removed program.");
				return true;
			}
			else if (Args.Contains("list") && Args.Contains("installed") && Args.Length == 2)
			{
				List<string> Packages = Client.InstalledPrograms;
				foreach(string Program in Packages)
				{
					if (Packages.Contains(Program) && ListPackages(GetPackages(Client)).Contains(Program))
					{
						Console.WriteLine(Program);
					}
				}
				return true;
			}
			else if (Args.Contains("list") && Args.Contains("remote") && Args.Length == 2)
			{
				List<string> Programs = ListPackages(GetPackages(Client));
				foreach(string Program in Programs)
				{
					Console.WriteLine(Program);
				}
				return true;
			}
			else
			{
				Console.WriteLine("usage: apt [subcommand] [options[...]]");
				Console.WriteLine("Subcommands:");
				Console.WriteLine("\tinstall [package] - Installs a package");
				Console.WriteLine("\tremove [package] - Removes a package");
				Console.WriteLine("\tlist [remote/installed] - Lists packages that are installed/available remotely");
				return false;
			}
		}
	}
}
