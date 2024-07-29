using ReHack.Node;
using ReHack.Node.PackageRepo;
using ReHack.BaseMethods;

namespace ReHack.Programs.Apt
{
	public static class Apt
	{
		public static PackageRepo GetRepo()
		{
			BaseNode Node = NodeUtils.GetNode("debian-pkg");
			return Node as PackageRepo;
		}
		/*public static List<Package> GetPackages(BaseNode Client)*/
		/*{}*/
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Contains("install") && Args.Length == 2)
			{
				string Program = Args.FirstOrDefault(Item => Item != "install");
				if (Client.InstalledPrograms.Contains(Program))
				{
					Console.WriteLine("error: Package is already installed");
					return false;
				}

				if (!GetRepo().ListPackages().Contains(Program))
				{
					Console.WriteLine("error: Package not found");
					return false;
				}

				Package PackageToInstall = GetRepo().GetPackage(Program);
				PackageToInstall.Install(Client);
				Console.WriteLine($"Package '{PackageToInstall.Name}' successfully installed.");

				return true;
			}
			else if (Args.Contains("remove") && Args.Length == 2)
			{
				string Program = Args.FirstOrDefault(Item => Item != "remove");
				PackageRepo Repo = GetRepo();
				if (!Repo.ListPackages().Contains(Program))
				{
					Console.WriteLine("error: Invalid package");
					return false;
				}
				if (!Client.InstalledPrograms.Contains(Program))
				{
					Console.WriteLine("error: Package not installed.");
				}
				Client.RemoveProgram(Program);
				Console.WriteLine("Successfully removed program.");
				return true;
			}
			else if (Args.Contains("list") && Args.Contains("installed") && Args.Length == 2)
			{
				List<string> Packages = GetRepo().ListPackages();
				foreach(string Program in Client.InstalledPrograms)
				{
					if (Packages.Contains(Program))
					{
						Console.WriteLine(Program);
					}
				}
				return true;
			}
			else if (Args.Contains("list") && Args.Contains("remote") && Args.Length == 2)
			{
				foreach(string Program in GetRepo().ListPackages())
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
