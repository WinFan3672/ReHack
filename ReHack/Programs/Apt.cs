using ReHack.Node;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.Apt
{
	public static class Apt
	{
		public static Package[] GetRepoPackages(string Address)
		{
			PackageRepo Repo = NodeUtils.GetNodeByAddress(Address) as PackageRepo;
			return Repo.Packages;
		}
		public static List<Package> GetPackages(BaseNode Client)
		{
			List<Package> Packages = new List<Package>();
			foreach(string Repo in Client.Root.GetFile("/etc/apt/sources.list").Content.Split("\n"))
			{
				foreach(Package Item in GetRepoPackages(Repo))
				{
					Packages.Add(Item);
				}
			}
			return Packages;
		}
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
		public static List<string> ListPackages(List<Package> Packages)
		{
			List<string> Names = new List<string>();
			foreach(Package Item in Packages)
			{
				Names.Add(Item.Name);
			}
			return Names;
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Contains("install") && Args.Length == 2)
			{
				string Program = Args.FirstOrDefault(Item => Item != "install");
				if (Client.InstalledPrograms.Contains(Program))
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Package is already installed");
					return false;
				}


				Package? PackageToInstall = GetPackage(GetPackages(Client), Program);
				if (PackageToInstall == null)
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Invalid Package");
					return false;
				}

				PackageToInstall.Install(Client);
				Console.WriteLine($"Package '{PackageToInstall.Name}' successfully installed.");

				return true;
			}
			else if (Args.Contains("remove") && Args.Length == 2)
			{
				string Program = Args.FirstOrDefault(Item => Item != "remove");
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
