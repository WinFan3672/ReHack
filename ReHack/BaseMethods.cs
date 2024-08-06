using System.Text;
using System.Reflection;
using ReHack.Data;
using ReHack.Node;
using ReHack.Node.Mail;
using ReHack.Exceptions;

namespace ReHack.BaseMethods
{
	/// <summary>Useful functions for dealing with nodes.</summary>
	public static class NodeUtils
	{
		private static readonly Random Rand = new Random();

		/// <summary>Generates a random IP address</summary>
		public static string GenerateIPAddress(bool Local=false) {
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
		

		/// <summary>
		/// Returns a bool determining if a string is a valid URL.
		/// </summary>
		public static bool IsURL(string URL)
		{
			return URL.StartsWith("w3://");
		}
	

		/// <summary>
		/// Converts a URL (starting with 'w3://') 
		/// </summary>
		public static (string, string) DeconstructURL(string URL)
		{
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

		/// <summary>Checks that a node exists.</summary>
		/// <param name='UID'>The Unique ID (UID attribute) of a node.</param>
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

		/// <summary>
		/// Checks if a node exists.
		/// </summary>
		/// <param name="Address">The node's hostname (IP/domain address).</param>
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

		/// <summary>Returns a BaseNode from a UID.</summary>
		/// <param name="UID">The node's UID.</param>
		/// <param name="Force">If true, the node is returned regardless of if the node is alive or not.</param>
		public static BaseNode GetNode(string UID, bool Force=false)
		{
			foreach(var Node in GameData.Nodes)
			{
				if (Node.UID == UID && Node.CheckHealth() || Force)
				{
					return Node;
				}
			}
			throw new ArgumentException("Invalid node");

		}
		

		/// <summary>Returns a BaseNode from a UID.</summary>
		/// <param name="Address">The node's hostname (IP address or domain).</param>
		/// <param name="Force">If true, the node is returned regardless of if the node is alive or not.</param>
		public static BaseNode GetNodeByAddress(string Address, bool Force=false)
		{
			foreach(BaseNode Node in GameData.Nodes)
			{
				if (Node.Address == Address && Node.CheckHealth() || Force)
				{
					return Node;
				}
			}
			throw new ErrorMessageException("Invalid hostname");
		}

		/// <summary>Checks if a node has a port listening.</summary>
		/// <param name="Client">The node to check</param>
		/// <param name="ServiceID">The port to check based on its service ID</param>
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

		/// <summary>Returns a list of strings containing all node addresses.</summary>
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

	/// <summary>
	/// User class - each Node has an array of User objects which can be logged into.
	/// </summary>
	public class User {
		/// <summary>The username.</summary>
		public string Username {get; set; }
		/// <summary>The user's password. Can be null to disallow logins.</summary>
		public string? Password {get; set; }
		/// <summary>If this is enabled, the user can perform privileged operations</summary>
		public bool Privileged {get; set; }
		/// <summary>If enabled, the user can use `sudo` to escalate privileges. Should be mutually exclusive with Privileged.</summary>
		public bool CanSudo {get; set; }

		/// <summary>Constructor.</summary>
		public User(string Username, string? Password, bool Privileged, bool CanSudo)
		{
			this.Username = Username;
			this.Password = Password;
			this.Privileged = Privileged;
			this.CanSudo = CanSudo;
		}

	}

	/// <summary>Collection of useful functions for dealing with instances of User.</summary>
	public static class UserUtils
	{
		/// <summary>
		/// Returns a random password that the game *can* brute-force.
		/// </summary>
		public static string PickPassword() {
			Random Rand = new Random();
			return GameData.Passwords[Rand.Next(GameData.Passwords.Length)];
		}

		/// <summary>Returns a bool depending on if a username is disallowed.</summary>
		/// <param name="Username">Username to check.</param>
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

		/// <summary>
		/// Asks the user to enter a name and password.
		/// </summary>
		/// <param name="BanCheck">Dictates whether to check GameData.BannedUsernames</param>
		public static (string, string) GetCredentials(bool BanCheck=true)
		{
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

		/// <summary>Makes a port on a node open.</summary>
		public static void OpenPort(BaseNode Client, string ServiceID)
		{
			Port Service = Client.GetPort(ServiceID);
			Service.Open = true;
		}

		/// <summary>Makes a port on a node closed.</summary>
		public static void ClosePort(BaseNode Client, string ServiceID)
		{
			Port Service = Client.GetPort(ServiceID);
			Service.Open = false;
		}
	}

	/// <summary>
	/// Email class - defines a recipient, sender, subject, and content
	/// </summary>
	public class Email {
		/// <summary>The email address of the person receiving the email.</summary>
		public string Recipient {get; set; }
		/// <summary>The email address of the person sending the email.</summary>
		public string Sender {get; set; }
		/// <summary>The subject line of the email.</summary>
		public string Subject {get; set; }
		/// <summary>The body of the email.</summary>
		public string Content {get; set; }
		// public Link[] Links {get; set; }

		/// <summary>Constructor</summary>
		public Email(string Recipient, string Sender, string Subject, string Content) {
			this.Recipient = Recipient;
			this.Sender = Sender;
			this.Subject = Subject;
			this.Content = Content;
		}
	}


	/// <summary>
	/// Port - Has a service name and ID, port number, and can be open/closed.
	/// </summary>
	public class Port {
		/// <summary>The human name of the port</summary>
		public string ServiceName {get; set; }
		/// <summary>The unique ID used to retrieve the port in code.</summary>
		public string ServiceID {get; set; }
		/// <summary>The default number for the port. If a single node changes the port number from this default, don't worry.</summary>
		public int PortNumber {get; set; }
		/// <summary>Whether the port is open.</summary>
		public bool Open {get; set;}

		/// <summary>Constructor</summary>
		public Port(string ServiceName, string ServiceID, int PortNumber, bool Open=true)
		{
			this.ServiceName = ServiceName;
			this.ServiceID = ServiceID;
			this.PortNumber = PortNumber;
			this.Open = Open;
		}

		/// <summary>Returns a copy of the port</summary>
		public Port Copy()
		{
			return new Port(ServiceName, ServiceID, PortNumber, Open);
		}
	}

	/// <summary>Collection of utilities for use with the console</summary>
	public static class PrintUtils
	{
		/// <summary>Prints a divider element</summary>
		public static void Divider()
		{
			Console.WriteLine("--------------------");
		}

		/// <summary>Asks the user to enter a password. Looks very cool because of the asterisks.</summary>
		/// <param name="ShowAsterisks">Whether to print asterisks when a character is typed.</param>
		public static string ReadPassword(bool ShowAsterisks=true)
		{
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
					if (ShowAsterisks)
					{
						Console.Write("\b \b"); // Erase the last * from the console
					}
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

		/// <summary>Asks the user to confirm.</summary>
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
		/// <summary>Formats some text centered.</summary>
		/// <param name="text">The text to print.</param>
		/// <param name="width">How many characters wide the text is. For simplicity's sake, pass in Console.WindowWidth.</param>
		/// <param name="FillChar">The character that fills in whitespace around the text.</param>
		/// <param name="EndChar">The first and last characters.</param>
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

		/// <summary>Passes arguments into FormatCentered and prints the result. Legacy function.</summary>
		public static void PrintCentered(string text, int width, char FillChar=' ', char EndChar=' ')
		{
			Console.WriteLine(FormatCentered(text, width, FillChar, EndChar));
		}

		/// <summary>Formats some text underlined.</summary>
		public static string FormatUnderlined(string text)
		{
			// ANSI escape codes for underlined text
			const string underlineOn = "\x1b[4m";
			const string underlineOff = "\x1b[0m";
			
			return $"{underlineOn}{text}{underlineOff}";
		}

		/// <summary>Shows a 'Press any key to continue' screen.</summary>
		public static void WaitForContinue()
		{
			Console.Write("Press any key to continue.");
			Console.ReadKey();
		}
	}

	/// <summary>Utilities for dealing with files embedded in the assembly.</summary>
	public static class FileUtils
	{
		/// <summary>Takes the path of an embedded file and returns its contents.</summary>
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

		/// <summary>Generates a facsimile of what a binary file looks like</summary>
		public static string GenerateBytes(int Width=64, int Height=8)
		{
			string Bytes = "";
			Random Rand = new Random();
			for (int i = 0; i < Height; i++)
			{
				for (int x = 0; x < Width; x++)
				{
					Bytes = Bytes + Rand.Next(2);

				}
				Bytes = Bytes + "\n";
			}

			return Bytes.TrimEnd('\n');
		}
	}

	/// <summary>The delegate for a program function.</summary>
	public delegate bool ProgramDelegate(string[] Args, BaseNode Player, User RunningUser);

	/// <summary>A program. Defines its name, description, manpages, and method.</summary>
	public class ProgramDefinition
	{
		/// <summary>Program's name.</summary>
		public string Name {get; set; }
		/// <summary>The text shown in the help menu.</summary>
		public string Description {get; set; }
		/// <summary>The method to call when running the program.</summary>
		public ProgramDelegate Method {get; set; }
		/// <summary>The list of manpages associated with the program.</summary>
		public string[] Manpages {get; set; }

		/// <summary>Constructor.</summary>
		public ProgramDefinition(string Name, string Description, ProgramDelegate Method, string[] Manpages)
		{
			this.Name = Name;
			this.Description = Description;
			this.Method = Method;
			this.Manpages = Manpages;
		}
	}

	/// <summary>Functions relating to release and debug mode.</summary>
	public static class DebugUtils
	{
		/// <summary>Returns true if the REHACK_DEBUG environment variable is set, and returns false if the program was compiled in release mode.</summary>
		public static bool IsDebug()
		{
			#if !DEBUG
				return false;
			#else
				return System.Environment.GetEnvironmentVariables().Contains("REHACK_DEBUG");
			#endif
		}
	}

	/// <summary>Functions relating to emails.</summary>
	public static class EmailUtils
	{
		/// <summary>
		/// Returns a bool depending on if an email address is valid.
		/// </summary>
		public static bool IsValid(string EmailAddress)
		{
			if (EmailAddress == "") { return false; }

			if (!EmailAddress.Contains("@")) { return false; }

			if (EmailAddress.Split("@").Length != 2) { return false; }

			return true;
		}

		/// <summary>
		/// Returns the username of an email address.
		/// </summary>
		public static string GetUsername(string EmailAddress)
		{
			return EmailAddress.Split("@")[0];
		}

		/// <summary>
		/// Returns the domain of an email address.
		/// </summary>
		public static string GetDomain(string EmailAddress)
		{
			return EmailAddress.Split("@")[1];
		}

		/// <summary>Sends an email and returns a status code.</summary>
		/// <return>0: Email sent
		/// 1: Invalid receiving address
		/// 2: Invalid mail server
		/// 3: Invalid user</return>
		public static int SendEmail(Email Eml)
		{
			
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

	/// <summary>Miscellaneous functions.</summary>
	public static class MiscUtils
	{
		/// <summary>
		/// Takes an array and an item and returns a new array with the new item added.
		/// </summary>
		public static T[] AddItemToArray<T>(T[] originalArray, T newItem)
		{
			T[] newArray = new T[originalArray.Length + 1];
			for (int i = 0; i < originalArray.Length; i++)
			{
				newArray[i] = originalArray[i];
			}
			newArray[originalArray.Length] = newItem;
			return newArray;
		}

	}

	/// <summary>A bank transaction.</summary>
	public class BankTransaction
	{
		/// <summary>The IP address of the person receiving the money.</summary>
		public string ToAddress {get; set; }
		/// <summary>The IP address of the person sending the money.</summary>
		public string FromAddress {get; set; }
		/// <summary>The amount of money sent.</summary>
		public int Amount {get; set; }
		/// <summary>The reason why the money is being sent.</summary>
		public string Reason {get; set; }

		/// <summary>Constructor.</summary>
		public BankTransaction(string FromAddress, string ToAddress, int Amount, string Reason)
		{
			this.ToAddress = ToAddress;
			this.FromAddress = FromAddress;
			if (Amount < 0)
			{
				throw new ArgumentException("Negative balances disallowed");
			}
			this.Amount = Amount;
			this.Reason = Reason;
		}

		/// <summary>Formats as a string.</summary>
		public override string ToString()
		{
			return $"{FromAddress} --> {ToAddress}\t{Amount}\t{Reason}";
		}
	}

	/// <summary>Functions relating to bank transactions.</summary>
	public static class BankUtils
	{
		/// <summary>Performs a bank transaction.</summary>
		public static bool PerformTransaction(BankTransaction Order)
		{
			BaseNode FromNode = NodeUtils.GetNodeByAddress(Order.FromAddress);
			BaseNode ToNode = NodeUtils.GetNodeByAddress(Order.ToAddress);

			if (FromNode.Balance < Order.Amount)
			{
				return false;
			}

			FromNode.Balance -= Order.Amount;
			ToNode.Balance += Order.Amount;
			FromNode.MoneySent.Add(Order);
			ToNode.MoneyReceived.Add(Order);
			FromNode.MoneyHandled.Add(Order);
			ToNode.MoneyHandled.Add(Order);

			return true;
		}

		/// <summary>Checks that a node's transactions 'make sense', i.e the current balance is equal to how much money was sent and received.</summary>
		public static bool ValidateTransactions(BaseNode Node)
		{
			int Balance = 0;
			foreach (BankTransaction Transaction in Node.MoneyReceived)
			{
				Balance += Transaction.Amount;
			}
			foreach (BankTransaction Transaction in Node.MoneySent)
			{
				Balance -= Transaction.Amount;
			}

			return (Balance == Node.Balance);
		}
	}

}
