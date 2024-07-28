using System.Text;
using System.IO;
using System.Reflection;

using ReHack.Data;
using ReHack.Node;

namespace ReHack.BaseMethods
{
    public static class NodeUtils
    {
        private static readonly Random Rand = new Random();

        public static string GenerateIPAddress() {
            /// <summary>
            /// Generates a random IP address
            /// </summary>
            string Address =  $"{Rand.Next(0, 256)}.{Rand.Next(0, 256)}.{Rand.Next(0, 256)}.{Rand.Next(0, 256)}";
            foreach (var Node in GameData.Nodes)
            {
                if (Node.Address == Address)
                {
                    return GenerateIPAddress();
                }
            }
            return Address;
        }

        public static BaseNode GetNode(string UID)
        {
            foreach(var Node in GameData.Nodes)
            {
                if (Node.UID == UID)
                {
                    return Node;
                }
            }
            throw new Exception("Invalid node");
        }

	public static BaseNode GetNodeByAddress(string Address)
	{
		foreach(BaseNode Node in GameData.Nodes)
		{
			if (Node.Address == Address)
			{
				return Node;
			}
		}
		throw new Exception("Invalid node");
	}

        public static bool CheckPort(BaseNode Client, string ServiceID)
        {
            Port CheckService = GameData.GetPort(ServiceID);
            foreach(Port Service in Client.Ports)
            {
                if (Service.ServiceID == CheckService.ServiceID)
                {
                    return true;
                }
            }
            return false;
        }

	public static List<string> GetAddressList()
	{
		List<string> Addresses = new List<string>();
		foreach(BaseNode Node in GameData.Nodes)
		{
			Addresses.Add(Node.Address);
		}
		return Addresses;
	}
    }

    public class User {
        /// <summary>
        /// User class - each Node has an array of User objects which can be logged into.
        /// </summary>
        public string Username {get; set; }
        public string? Password {get; set; }
        public bool Privileged {get; set; } // If this is enabled, the user can perform privileged operations

        public User(string Username, string Password, bool Privileged)
        {
            this.Username = Username;
            this.Password = Password;
            this.Privileged = Privileged;
        }

    }

    public static class UserUtils
    {
        public static string PickPassword() {
            /// <summary>
            /// Returns a random password that the game *can* brute-force.
            /// </summary>
            return "root"; // Just a stub
        }
        
        public static bool IsUsernameBanned(string Username)
        {
            foreach (string Banned in GameData.BannedUsernames)
            {
                if (Username.ToLower() == Banned.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public static (string, string) GetCredentials(bool BanCheck=true)
        {
            /// <summary>
            /// Asks the user to enter a name and password. The BanCheck value dictates whether to check GameData.BannedUsernames
            /// </summary>
            bool Banned;
            string Username;
            string Password = "";
            while (true)
            {
                Banned = false;
                Console.Write("Enter A Username $");
                Username = Console.ReadLine().ToLower();
                if (BanCheck)
                {
                    if (IsUsernameBanned(Username))
                    {
                        Banned = true;
                        Console.WriteLine("ERROR: You can't have that username.");
                    }
                }
                if (!Banned)
                {
                    break;
                }

            }

            while (Password == "")
            {
                Console.Write("Enter A Password $");
                Password = PrintUtils.ReadPassword();
            }
            return (Username, Password);
        }
    }

    public class Email {
        /// <summary>
        /// Email class - defines a recipient, sender, subject, and content
        /// </summary>
        public string Recipient {get; set; }
        public string Sender {get; set; }
        public string Subject {get; set; }
        public string Content {get; set; }
        // public Link[] Links {get; set; }

        public Email(string Recipient, string Sender, string Subject, string Content) {
            this.Recipient = Recipient;
            this.Sender = Sender;
            this.Subject = Subject;
            this.Content = Content;
        }

        public bool Send()
        {
            return false;
        }
    }

    public class Port {
        /// <summary>
        /// Port - Has a service name and ID, port number, and can be open/closed.
        /// </summary>
        public string ServiceName {get; set; }
        public string ServiceID {get; set; }
        public int PortNumber {get; set; }
        public bool Open {get; set;}

        public Port(string ServiceName, string ServiceID, int PortNumber, bool Open=false)
        {
            this.ServiceName = ServiceName;
            this.ServiceID = ServiceID;
            this.PortNumber = PortNumber;
            this.Open = Open;
        }
    }

    public static class PrintUtils
    {
        public static void Divider()
        {
            Console.WriteLine("--------------------");
        }

        public static string ReadPassword()
        {
            /// <summary>
            /// Basic password entry function. Reads passwords from stdin, showing asterisks 
            /// </summary>
            StringBuilder password = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                    Console.Write("\b \b"); // Erase the last * from the console
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                    /*Console.Write('\u2022');*/
					Console.Write("*");
                }
            }
            Console.WriteLine();
            return password.ToString();
        }

		private static string GetAgreeString(bool DefaultAgree)
		{
			if (DefaultAgree)
			{
				return "[Y/n]";
			}
			else
			{
				return "[y/N]";
			}
			
		}
			
		public static bool Confirm(string Message, bool DefaultAgree=true)
		{
			string Confirmation;
			while (true)
			{
				Console.Write($"{Message} {GetAgreeString(DefaultAgree)} $");
				Confirmation = Console.ReadLine().ToLower();
				if (Confirmation == "y")
				{
					return true;
				}
				else if (Confirmation == "n")
				{
					return false;
				}
				else if (Confirmation == "")
				{
					return DefaultAgree;
				}
				else {
					Console.WriteLine("ERROR: Invalid choice.");
					return Confirm(Message, DefaultAgree);
				}
			}
		}
    }

	public static class FileUtils
	{
		public static string GetFileContents(string Filename)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string Name = $"ReHack.Embedded.{Filename}";

			using (var Stream = assembly.GetManifestResourceStream(Name))
			{
				return new StreamReader(Stream).ReadToEnd();
			}
		}
	}
	
	public delegate bool ProgramDelegate(string[] Args, BaseNode Player, User RunningUser);

	public class ProgramDefinition
	{
		public string Name {get; set; }
		public string Description {get; set; }
		public ProgramDelegate Method {get; set; }

		public ProgramDefinition(string Name, string Description, ProgramDelegate Method)
		{
			this.Name = Name;
			this.Description = Description;
			this.Method = Method;
		}
	}

	public static class DebugUtils
	{
		public static bool IsDebug()
		{
			return System.Environment.GetEnvironmentVariables().Contains("REHACK_DEBUG");
		}
	}

}
