using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;
using ReHack.Exceptions;
using ReHack.Programs.SSH;

namespace ReHack.Programs.Metasploit
{
	public static class MetaHack
	{
		public static void RatShell(BaseNode Target, User TargetUser)
		{
			string Prompt;
			AnsiConsole.MarkupLine($"Established remote connection with {TargetUser.Username}@{Target.Address}!");
			while (true)
			{
				Console.Write("(metahack-rat) $");
				Prompt = Console.ReadLine() ?? throw new EndOfStreamException();
				if (Prompt == "help")
				{
					foreach(string Line in new[] {
							"[blue]help[/] - Lists commands",
							"[blue]shell[/] - Launches a reverse shell",
							})
					{
						AnsiConsole.MarkupLine(Line);
					}
				}
				else if (Prompt == "shell")
				{
					SSHClient.ServiceRunner(Target, TargetUser, false, false);
				}
				else if (Prompt == "exit" || Prompt == "quit")
				{
					if (AnsiConsole.Confirm("Are you sure you want to exit? This will kill the session"))
					{
						return;
					}
				}
				else
				{
					SSHClient.RunCommand(Target, Prompt, TargetUser);
				}
			}
		}

		public static string[] GetShellExtensions(BaseNode Client)
		{
			List<string> Extensions = new List<string>();
			foreach(string Extension in new[] {"metahack-telnet", "metahack-ssh"})
			{
				if (Client.ShellExtensions.Contains(Extension))
				{
					Extensions.Add(Extension);
				}
			}
			return Extensions.ToArray();
		}
		public static string GetFriendlyName(string ShellExtension)
		{
			if (ShellExtension == "metahack-telnet")
			{
				return "Telnet Tools";
			}
			else if (ShellExtension == "metahack-ssh")
			{
				return "SSH Tools";
			}
			else
			{
				throw new ArgumentException("Invalid shell extension");
			}
		}
		public static string[] GetFriendlyNames(string[] ShellExtensions, bool AddExit=false)
		{
			List<string> Extensions = new List<string>();
			foreach(string Ext in ShellExtensions)
			{
				Extensions.Add(GetFriendlyName(Ext));
			}
			if (AddExit)
			{
				Extensions.Add("Exit");
			}
			return Extensions.ToArray();
		}
		private static void TelnetTools(BaseNode Client, User RunningUser)
		{
			string[] Choices = new string[] {
				"Telnet Upload RAT",
			};

			string Selection = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Telnet Tools").AddChoices(Choices));
			switch (Selection)
			{
				case "Telnet Upload RAT":
					string TargetAddress = AnsiConsole.Ask<string>("Target hostname: ");
					BaseNode Target = NodeUtils.GetNodeByAddress(TargetAddress);
					AnsiConsole.MarkupLine("Siphoning users list from server...");
					Thread.Sleep(500);
					User TargetUser = Target.GetUser(AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select User").AddChoices(Target.ListUsers())));
					AnsiConsole.MarkupLine("Stealing password...");
					Thread.Sleep(750);
					if (TargetUser.Password == null)
					{
						throw new ErrorMessageException("User has no password, theft cannot occur");
					}
					else
					{
						RatShell(Target, TargetUser);
					}
					break;
				default:
					break;
			}
		}
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (!NodeUtils.CheckPort(Client, "c2"))
			{
				throw new ErrorMessageException("The command-and-control software is not running. Use the [yellow]c2d[/] software to start it.");
			}
			Console.Clear();
			string Selection = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Metahack").AddChoices(GetFriendlyNames(GetShellExtensions(Client), true)));
			switch (Selection)
			{
				case "Telnet Tools":
					TelnetTools(Client, RunningUser);
					break;
				case "Exit":
					break;
				default:
					throw new ErrorMessageException("Invalid choice");
			}
			return true;
		}
	}
}
