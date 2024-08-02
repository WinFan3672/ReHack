using System.Text;
using System.Reflection;
using ReHack.Data;
using ReHack.Node;
using ReHack.Node.Mail;

namespace ReHack.BaseMethods
{
	public static class NodeUtils
	{
		private static readonly Random Rand = new Random();

		public static string GenerateIPAddress(bool Local=false) {
			/// <summary>
			/// Generates a random IP address
			/// </summary>
			Random Rand = new Random();
			int A;
			if (Local)
			{
				A = 10;
			}
			else
			{
				A = Rand.Next(0, 256);
			}
			while (!Local && A == 10)
			{
				A = Rand.Next(0, 256);
			}
			string Address =  $"{A}.{Rand.Next(0, 256)}.{Rand.Next(0, 256)}.{Rand.Next(0, 256)}";
			foreach (var Node in GameData.Nodes)
			{
				if (Node.Address == Address)
				{
					return GenerateIPAddress();
				}
			}
			return Address;
		}

		public static bool IsURL(string URL)
		{
			/// <summary>
			/// Returns a bool determining if a string is a valid URL.
			/// </summary>
			return URL.StartsWith("w3://");
		}
		
		public static (string, string) DeconstructURL(string URL)
		{
			/// <summary>
			/// Converts a URL (starting with 'w3://') 
			/// </summary>
			if (!IsURL(URL))
			{
				throw new ArgumentException("Invalid URL");
			}
			URL = URL.Replace("w3://", "");
			string[] Parts = URL.Split('/');

			if (Parts.Length == 1 || Parts.Length == 2 && Parts[1] == "")
			{
				return (Parts[0], "/");
			}
			else if (Parts.Length == 2)
			{
				return (Parts[0], Parts[1]);
			}
			else
			{
				throw new ArgumentException("URL has too many parts");
			}
		}

		public static bool CheckNode(string UID)
		{
			foreach(var Node in GameData.Nodes)
			{
				if (Node.UID == UID)
				{
					return true;
				}
			}
			return false;
		}

		public static bool CheckNodeByAddress(string Address)
		{
			foreach (var Node in GameData.Nodes)
			{
				if (Node.Address == Address)
				{
					return true;
				}

			}
			return false;
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
			throw new ArgumentException("Invalid node");

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
			throw new ArgumentException("Invalid node");
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

		public User(string Username, string? Password, bool Privileged)
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
			Random Rand = new Random();
			return GameData.Passwords[Rand.Next(GameData.Passwords.Length)];
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
			string? Password = "";
			while (true)
			{
				Banned = false;
				Console.Write("Enter A Username $");
				Username = Console.ReadLine() ?? throw new EndOfStreamException();
				Username = Username.ToLower();
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

		public static void OpenPort(BaseNode Client, string ServiceID)
		{
			Port Service = Client.GetPort(ServiceID);
			Service.Open = true;
		}

		public static void ClosePort(BaseNode Client, string ServiceID)
		{
			Port Service = Client.GetPort(ServiceID);
			Service.Open = false;
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

		public Port(string ServiceName, string ServiceID, int PortNumber, bool Open=true)
		{
			this.ServiceName = ServiceName;
			this.ServiceID = ServiceID;
			this.PortNumber = PortNumber;
			this.Open = Open;
		}

		public Port Copy()
		{
			return new Port(ServiceName, ServiceID, PortNumber, Open);
		}
	}

	public static class PrintUtils
	{
		public static void Divider()
		{
			Console.WriteLine("--------------------");
		}

		public static string ReadPassword(bool ShowAsterisks=true)
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
					if (ShowAsterisks)
					{
						Console.Write("*");
					}
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
			string? Confirmation;
			while (true)
			{
				Console.Write($"{Message} {GetAgreeString(DefaultAgree)} $");
				Confirmation = Console.ReadLine();
				if (Confirmation == null || Confirmation == "")
				{
					return DefaultAgree;
				}
				else if (Confirmation.ToLower() == "y")
				{
					return true;
				}
				else if (Confirmation.ToLower() == "n")
				{
					return false;
				}
				else {
					Console.WriteLine("ERROR: Invalid choice.");
					return Confirm(Message, DefaultAgree);
				}
			}
		}

		public static string FormatCentered(string text, int width, char FillChar=' ', char EndChar=' ')
		{
			if (width < text.Length - 2)
			{
				throw new ArgumentException("Text too wide");
			}
			
			if (FillChar != ' ' || EndChar != ' ')
			{
				width = width - 6;
			}


			int spaces = width - text.Length;
			int leftPadding = spaces / 2;
			leftPadding = leftPadding - 2;
			int rightPadding = spaces - leftPadding;

			return EndChar + new string(FillChar, leftPadding) + EndChar +  ' ' + text + ' ' + EndChar + new string(FillChar, rightPadding) + EndChar;
		}
		public static void PrintCentered(string text, int width, char FillChar=' ', char EndChar=' ')
		{
			Console.WriteLine(FormatCentered(text, width, FillChar, EndChar));
		}

		public static string FormatUnderlined(string text)
		{
			// ANSI escape codes for underlined text
			const string underlineOn = "\x1b[4m";
			const string underlineOff = "\x1b[0m";
			
			return $"{underlineOn}{text}{underlineOff}";
		}

		public static void WaitForContinue()
		{
			Console.Write("Press any key to continue.");
			Console.ReadKey();
		}
	}

	public static class FileUtils
	{
		public static string? GetFileContents(string Filename)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string Name = $"ReHack.Embedded.{Filename}";

			if (!assembly.GetManifestResourceNames().Contains(Name))
			{
				throw new ArgumentException("Invalid file");
			}

			using (System.IO.Stream? Stream = assembly.GetManifestResourceStream(Name) ?? throw new ArgumentException("Stream is null"))
			{
				return new StreamReader(Stream).ReadToEnd() ?? null;
			}
		}
	}

	public delegate bool ProgramDelegate(string[] Args, BaseNode Player, User RunningUser);

	public class ProgramDefinition
	{
		public string Name {get; set; }
		public string Description {get; set; }
		public ProgramDelegate Method {get; set; }
		public string[] Manpages {get; set; }

		public ProgramDefinition(string Name, string Description, ProgramDelegate Method, string[] Manpages)
		{
			this.Name = Name;
			this.Description = Description;
			this.Method = Method;
			this.Manpages = Manpages;
		}
	}

	public static class DebugUtils
	{
		public static bool IsDebug()
		{
			#if !DEBUG
				return false;
			#else
				return System.Environment.GetEnvironmentVariables().Contains("REHACK_DEBUG");
			#endif
		}
	}

	public static class EmailUtils
	{
		public static bool IsValid(string EmailAddress)
		{
			/// <summary>
			/// Returns a bool depending on if an email address is valid.
			/// </summary>
			if (EmailAddress == "") { return false; }

			if (!EmailAddress.Contains("@")) { return false; }

			if (EmailAddress.Split("@").Length != 2) { return false; }

			return true;
		}

		public static string GetUsername(string EmailAddress)
		{
			/// <summary>
			/// Returns the username of an email address.
			/// </summary>
			return EmailAddress.Split("@")[0];
		}

		public static string GetDomain(string EmailAddress)
		{
			/// <summary>
			/// Returns the domain of an email address.
			/// </summary>
			return EmailAddress.Split("@")[1];
		}

		public static int SendEmail(Email Eml)
		{
			/// <summary>
			/// Sends an email and returns a status code:
			/// 0: Email sent
			/// 1: Invalid receiving address
			/// 2: Invalid mail server
			/// 3: Invalid user
			/// </summary>
			
			if (!IsValid(Eml.Recipient))
			{
				return 1; // Invalid receiving address
			}

			MailServer? TargetServer = NodeUtils.GetNodeByAddress(GetDomain(Eml.Recipient)) as MailServer;

			if (TargetServer == null)
			{
				return 2;
			}

			MailAccount? Account = TargetServer.GetAccount(GetUsername(Eml.Recipient));

			if (Account == null)
			{
				return 3;
			}

			Account.Inbox.Add(Eml);
			return 0;
		}
	}

	public static class MiscUtils
	{
		public static T[] AddItemToArray<T>(T[] originalArray, T newItem)
		{
			/// <summary>
			/// Takes an array and an item and returns a new array with the new item added.
			/// </summary>
			T[] newArray = new T[originalArray.Length + 1];
			for (int i = 0; i < originalArray.Length; i++)
			{
				newArray[i] = originalArray[i];
			}
			newArray[originalArray.Length] = newItem;
			return newArray;
		}

	}

}
