using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;
using ReHack.Data;
using ReHack.Programs.SSH;

namespace ReHack.Programs.Hydra
{
	public static class Hydra
	{
		public static void ShowUsage()
		{
			Console.WriteLine("usage: hydra [user]@[hostname]");
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length != 1)
			{
				ShowUsage();
				return false;
			}

			if (!Args[0].Contains("@"))
			{
				ShowUsage();
				return false;
			}

			if (Args[0].Split("@").Length != 2)
			{
				ShowUsage();
				return false;
			}

			string[] UserAndHost = Args[0].Split("@");

			string Username = UserAndHost[0];
			string Hostname = UserAndHost[1];

			BaseNode NodeToHack;

			try
			{
				NodeToHack = NodeUtils.GetNodeByAddress(Hostname);
			}
			catch (ArgumentException)
			{
				Console.WriteLine("error: Invalid hostname");
				return false;
			}

			User UserToHack;

			try
			{
				UserToHack = NodeToHack.GetUser(Username);
			}
			catch (ArgumentException)
			{
				Console.WriteLine("error: Invalid username");
				return false;
			}

			Console.WriteLine("Begin brute-force...");
			bool Found = false;
			foreach(string Password in GameData.Passwords)
			{
				if (Password == UserToHack.Password)
				{
					Console.WriteLine($"Found password: {Password}");
					Found = true;
					break;
				}
			}
			if (!Found)
			{
				Console.WriteLine("error: No password found.");
				return false;
			}
			else
			{
				string YesNo = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Connect now?").AddChoices(new[] {"Yes", "No"}));
				
				if (YesNo == "Yes")
				{
					SSHClient.ServiceRunner(NodeToHack, UserToHack, false, false);
				}

				return true;
			}

		}
	}
}

