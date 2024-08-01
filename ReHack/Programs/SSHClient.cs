using ReHack.Data;
using ReHack.Data.Programs;
using ReHack.BaseMethods;
using ReHack.Node;
using Spectre.Console;

namespace ReHack.Programs.SSH
{
    public static class SSHClient
    {
		public static void RunCommand(BaseNode Client, string Command, User RunningUser)
		{
			string[] CommandPlusArgs = Command.Split(" ");

			if (CommandPlusArgs.Length == 0)
			{
				return;
			}

			string CommandName = CommandPlusArgs[0];
			string[] Args = CommandPlusArgs.Skip(1).ToArray();

			if (Client.ListPrograms().Contains(CommandName))
			{
				var Program = ProgramData.GetProgram(CommandName);
				Program.Method(Args, Client, RunningUser);
			}
			else {
				Console.WriteLine("error: Bad command");
			}
		}
		private static bool Authenticate(BaseNode Client, string Username)
		{
			User Person = Client.GetUser(Username);
			Console.Write($"ssh: {Username}@{Client.Address}'s password: ");
			string Password = PrintUtils.ReadPassword();
			return Password == Person.Password;
		}
        public static void ServiceRunner(BaseNode Client, User Person, bool ConfirmExit=false, bool DoAuthenticate = true)
        {
			if (DoAuthenticate)
			{
				if (!Authenticate(Client, Person.Username))
				{
					Console.WriteLine("Permission denied (password)");
					return;
				}
			}
			Client.Welcome();
            string Input;
            while (true)
            {
                Console.Write($"{Person.Username}@{Client.Address} $");
                Input = Console.ReadLine() ?? "";
                if (Input == "exit" || Input == "quit")
                {
					if (!ConfirmExit || PrintUtils.Confirm("Are you sure you want to exit?", false))
					{
						return;
					}
                }
				else if (Input == "clear" || Input == "cls")
				{
					Console.Clear();
				}
				else if (Input == "")
				{
				}
                else {
					try
					{
						RunCommand(Client, Input, Person);
					}
					catch (Exception ex)
					{
						AnsiConsole.WriteException(ex);
					}
                }
            }
        }
		private static bool Check(BaseNode Client, string Username)
		{
			return NodeUtils.CheckPort(Client, "ssh") && Authenticate(Client, Username);
		}
		private static void ShowUsage()
		{
				Console.WriteLine("usage: ssh [user]@[hostname]");
		}
		public static bool Program(string[] Args, BaseNode Player, User RunningUser)
		{
			if (Args.Length == 1)
			{
				if (!Args[0].Contains("@"))
				{
					ShowUsage();
					return false;
				}
				
				string[] UserAndHost = Args[0].Split("@");

				if (UserAndHost.Length != 2)
				{
					ShowUsage();
					return false;
				}

				string Username = UserAndHost[0];
				string Hostname = UserAndHost[1];
					
				BaseNode Host;

				try
				{
					Host = NodeUtils.GetNodeByAddress(Hostname);
				}
				catch
				{
					Console.WriteLine($"error: Invalid hostname ({Hostname})");
					return false;
				}

				if (!NodeUtils.CheckPort(Host, "ssh"))
				{
					Console.WriteLine("error: Connection refused");
					return false;
				}
					
				User HostUser;

				try
				{
					HostUser = Host.GetUser(Username);
				}
				catch
				{
					Console.WriteLine("error: Server reported invalid user");
					return false;
				}

				ServiceRunner(Host, HostUser);
				return true;
			}
			else
			{
				ShowUsage();
				return false;
			}
		}
    }
}
