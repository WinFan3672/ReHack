using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Data.Programs;
using ReHack.Filesystem;
using ReHack.Networks;
using ReHack.Exceptions;
using Spectre.Console;

namespace ReHack.Node {
	public class BaseNode {
		public string Name {get; set; }
		public string UID {get; set; }
		public string Address {get; set; }
		public User[] Users{get; set; }
		public List<Port> Ports {get; }
		public List<ProgramDefinition> Programs {get; } = new List<ProgramDefinition>();
		public List<string> Manpages {get; } = new List<string>();
		public List<string> InstalledPrograms {get;} = new List<string>();
		public FileSystem Root {get; set; }
		public AreaNetwork? Network {get; set; }
		public bool Healthy {get; set; }
		public List<string> ShellExtensions {get; set; } /// <summary>A shell extension is a pseudo-program that real programs can check against to see if it is unlocked.</summary>

		public BaseNode(string Name, string UID, string Address, User[] Users, AreaNetwork? Network)
		{
			this.Name = Name;
			this.UID = UID;
			this.Address = Address;
			this.Users = Users;
			this.Ports = new List<Port>();
			this.Root = new FileSystem(new VirtualFile[]{}, GameData.DefaultDirs).Clone();
			this.Network = Network;
			this.ShellExtensions = new List<string>();
			this.Healthy = true;

			this.Init();
		}

		public void Init()
		{
			/// <summary>
			/// Initialisation function.
			/// </summary>
			foreach(string Program in GameData.DefaultPrograms)
			{
				this.AddProgram(Program);
			}

			foreach(string Manpage in GameData.DefaultManpages)
			{
				this.Manpages.Add(Manpage);
			}

			foreach (User NewUser in Users)
			{
				InitUser(NewUser);
			}

		}

		public Port GetPort(string PortID)
		{
			/// <summary>
			/// Returns a port in the port list.
			/// </summary>
			foreach (Port Service in this.Ports)
			{
				if (Service.ServiceID == PortID)
				{
					return Service;
				}
			}
			throw new ArgumentException("Invalid port");
		}

		public void AddPort(string PortID)
		{
			this.Ports.Add(GameData.GetPort(PortID));
		}

		public void RemovePort(string PortID)
		{
			for (int i=0; i < Ports.Count; i++)
			{
				Port Service = Ports[i];
				if (Service.ServiceID == PortID)
				{
					Ports.Remove(Service);
				}
			}
		}

		public User GetUser(string Username)
		{
			foreach (User Person in this.Users)
			{
				if (Username == Person.Username)
				{
					return Person;
				}
			}
			throw new ArgumentException("Invalid username");
		}

		public void AddProgram(string ProgramName)
		{
			ProgramDefinition Program = ProgramData.GetProgram(ProgramName);
			this.Programs.Add(Program);
			this.InstalledPrograms.Add(Program.Name);
			foreach (string Manpage in Program.Manpages)
			{
				this.Manpages.Add(Manpage);
			}
		}

		public void RemoveProgram(string ProgramName)
		{
			ProgramDefinition Program = ProgramData.GetProgram(ProgramName);
			this.Programs.Remove(Program);
			this.InstalledPrograms.Remove(Program.Name);
			foreach (string Manpage in Program.Manpages)
			{
				this.Manpages.Remove(Manpage);
			}
		}

		public List<string> ListPrograms()
		{
			List<string> Programs = new List<string>();
			foreach (var Program in this.Programs)
			{
				Programs.Add(Program.Name);
			}
			return Programs;
		}

		public List<string> ListManpages()
		{
			List<string> Manpages = new List<string>();
			foreach (string Manpage in this.Manpages)
			{
				Manpages.Add(Manpage);
			}
			return Manpages;
		}

		public void AddFile(string Path, VirtualFile File)
		{
			VirtualDirectory? Directory = this.Root.GetDirectory(Path);
			if (Directory == null)
			{
				throw new ArgumentException("Invalid path");
			}

			Directory.AddFile(File);
		}

		public void AddDirectory(string Path, VirtualDirectory Directory)
		{
			VirtualDirectory? Dir = this.Root.GetDirectory(Path);
			if (Dir == null)
			{
				throw new ArgumentException("Invalid path");
			}

			Dir.AddDirectory(Directory);
		}

		public virtual void Welcome()
		{
			AnsiConsole.MarkupLine("Welcome to [red]Debian Linux[/] [green]5.0.5[/]!");
			AnsiConsole.MarkupLine("The programs included with the Debian GNU/Linux system are free software");
			AnsiConsole.MarkupLine("the exact distribution terms for each program are described in the");
			AnsiConsole.MarkupLine("individual files in [blue]/usr/share/doc/*/copyright[/].");
			AnsiConsole.MarkupLine("Debian GNU/Linux comes with [bold]ABSOLUTELY NO WARRANTY[/], to the extent");
			AnsiConsole.MarkupLine("permitted by applicable law.");
		}

		public List<string> ListUsers()
		{
			List<string> Users = new List<string>();
			foreach(User Item in this.Users)
			{
				Users.Add(Item.Username);
			}
			return Users;
		}

		public void InitUser(User NewUser)
		{
			/// <summary>
			/// Initialises a user object by creating its home folder.
			/// </summary>
			Root.GetDirectory("/home").AddDirectory(new VirtualDirectory(NewUser.Username, new VirtualFile[]{}, new VirtualDirectory[]{}));
		}

		public bool Authenticate(User Auth)
		{
			Console.Write("Password $");
			string Password = PrintUtils.ReadPassword();
			return (Auth.Password == Password);
		}

		public void CheckPerms(User RunningUser)
		{
			if (!RunningUser.Privileged)
			{
				throw new ErrorMessageException("Permission denied");
			}
		}

		public virtual bool CheckHealth()
		{
			return Healthy;
		}
	}
}
