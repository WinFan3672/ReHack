using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Data.Programs;
using ReHack.Filesystem;
using ReHack.Networks;
using ReHack.Exceptions;
using Spectre.Console;

namespace ReHack.Node {
	/// <summary>A node. The base class for all other nodes.</summary>
	public class BaseNode {
		/// <summary>A node's hostname.</summary>
		public string Name {get; set; }
		/// <summary>A node's Unique ID or UID. Used in code to retrieve the node.</summary>
		public string UID {get; set; }
		/// <summary>The node's IP address or domain.</summary>
		public string Address {get; set; }
		/// <summary>The node's users.</summary>
		public User[] Users{get; set; }
		/// <summary>All ports available on the system.</summary>
		public List<Port> Ports {get; }
		/// <summary>All installed programs.</summary>
		public List<ProgramDefinition> Programs {get; } = new List<ProgramDefinition>();
		/// <summary>Installed manpages.</summary>
		public List<string> Manpages {get; } = new List<string>();
		/// <summary>A list of programs installed by program name. There for conveience mainly.</summary>
		public List<string> InstalledPrograms {get;} = new List<string>();
		/// <summary>The root filesystem.</summary>
		public FileSystem Root {get; set; }
		/// <summary>The current LAN.</summary>
		public AreaNetwork? Network {get; set; }
		/// <summary>A bool determining if the node is up. Used in CheckHealth() by default, but subclasses may replace it with a different check instead.</summary>
		public bool Healthy {get; set; }
		/// <summary>A shell extension is a pseudo-program that real programs can check against to see if it is unlocked.</summary>
		public List<string> ShellExtensions {get; set; }
		/// <summary>How much money the node has.</summary>
		public Int64 Balance {get; set; }
		/// <summary>Every bank transaction where the node sent money.</summary>
		public List<BankTransaction> MoneySent {get; set; }
		/// <summary>Every bank transaction where the node received money.</summary>
		public List<BankTransaction> MoneyReceived {get; set; }
		/// <summary>Every bank transaction performed by the node. </summary>
		public List<BankTransaction> MoneyHandled {get; set; }

		/// <summary>Constructor.</summary>
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
			this.Balance = 0;
			this.MoneySent = new List<BankTransaction>();
			this.MoneyReceived = new List<BankTransaction>();
			this.MoneyHandled = new List<BankTransaction>();

			this.Init();
		}

		/// <summary>
		/// Initialisation function.
		/// </summary>
		public void Init()
		{
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

		/// <summary>
		/// Returns a port in the port list.
		/// </summary>
		public Port GetPort(string PortID)
		{
			foreach (Port Service in this.Ports)
			{
				if (Service.ServiceID == PortID)
				{
					return Service;
				}
			}
			throw new ArgumentException("Invalid port");
		}

		/// <summary>Adds a port based on a port ID.</summary>
		public void AddPort(string PortID)
		{
			this.Ports.Add(GameData.GetPort(PortID));
		}

		/// <summary>Removes a port based on a port ID.</summary>
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

		/// <summary>Returns a user from a username.</summary>
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

		/// <summary>Adds a program based on a program name.</summary>
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

		/// <summary>Removes a program based on a program name.</summary>
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

		/// <summary>Returns a list of programs</summary>
		public List<string> ListPrograms()
		{
			List<string> Programs = new List<string>();
			foreach (var Program in this.Programs)
			{
				Programs.Add(Program.Name);
			}
			return Programs;
		}

		/// <summary>Returns a list of manpages</summary>
		public List<string> ListManpages()
		{
			List<string> Manpages = new List<string>();
			foreach (string Manpage in this.Manpages)
			{
				Manpages.Add(Manpage);
			}
			return Manpages;
		}

		/// <summary>Adds a file to a directory.</summary>
		public void AddFile(string Path, VirtualFile File)
		{
			VirtualDirectory? Directory = this.Root.GetDirectory(Path);
			if (Directory == null)
			{
				throw new ArgumentException("Invalid path");
			}

			Directory.AddFile(File);
		}

		/// <summary>Adds a directory to a directory.</summary>
		public void AddDirectory(string Path, VirtualDirectory Directory)
		{
			VirtualDirectory? Dir = this.Root.GetDirectory(Path);
			if (Dir == null)
			{
				throw new ArgumentException("Invalid path");
			}

			Dir.AddDirectory(Directory);
		}

		/// <summary>Prints the MOTD (Message of the Day)</summary>
		public virtual void Welcome()
		{
			AnsiConsole.MarkupLine("Welcome to [red]Debian Linux[/] [green]5.0.5[/]!");
			AnsiConsole.MarkupLine("The programs included with the Debian GNU/Linux system are free software");
			AnsiConsole.MarkupLine("the exact distribution terms for each program are described in the");
			AnsiConsole.MarkupLine("individual files in [blue]/usr/share/doc/*/copyright[/].");
			AnsiConsole.MarkupLine("Debian GNU/Linux comes with [bold]ABSOLUTELY NO WARRANTY[/], to the extent");
			AnsiConsole.MarkupLine("permitted by applicable law.");
		}

		/// <summary>Returns a list of users.</summary>
		public List<string> ListUsers()
		{
			List<string> Users = new List<string>();
			foreach(User Item in this.Users)
			{
				Users.Add(Item.Username);
			}
			return Users;
		}

		/// <summary>
		/// Initialises a user object by creating its home folder.
		/// </summary>
		public void InitUser(User NewUser)
		{
			Root.GetDirectory("/home").AddDirectory(new VirtualDirectory(NewUser.Username, new VirtualFile[]{}, new VirtualDirectory[]{}));
		}

		/// <summary>Asks the user to authenticate with a password.</summary>
		/// <return>If true, the user entered the correct password.</return>
		public bool Authenticate(User Auth)
		{
			Console.Write("Password $");
			string Password = PrintUtils.ReadPassword();
			return (Auth.Password == Password);
		}

		/// <summary>Throws an ErrorMessageException if the user isn't privileged.</summary>
		public void CheckPerms(User RunningUser)
		{
			if (!RunningUser.Privileged)
			{
				throw new ErrorMessageException("Permission denied");
			}
		}

		/// <summary>Returns whether the node is up or not.</summary>
		public virtual bool CheckHealth()
		{
			return Healthy;
		}
	}
}
