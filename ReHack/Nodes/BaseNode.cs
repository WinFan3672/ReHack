using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Data.Programs;
using ReHack.Filesystem;
using ReHack.Networks;

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
		public FileSystem Root {get; } = new FileSystem(new VirtualFile[]{}, GameData.DefaultDirs);
		public AreaNetwork? Network {get; set; }

		public BaseNode(string Name, string UID, string Address, User[] Users)
		{
			this.Name = Name;
			this.UID = UID;
			this.Address = Address;
			this.Users = Users;
			this.Ports = new List<Port>();

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

		}

		public Port GetPort(string PortID)
		{
			/// <summary>
			/// Returns a port in the port list
			/// </summary>
			foreach (Port Service in this.Ports)
			{
				if (Service.ServiceID == PortID)
				{
					return Service;
				}
			}
			throw new Exception("Invalid port");
		}

		public void DebugPorts()
		{
			foreach (Port Service in this.Ports)
			{
				PrintUtils.Divider();
				Console.WriteLine($"Port list for {this.Name}");
				PrintUtils.Divider();
				Console.WriteLine($"{Service.ServiceID} = {Service.PortNumber}");
				PrintUtils.Divider();
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
			throw new Exception("Invalid username");
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
	}
}
