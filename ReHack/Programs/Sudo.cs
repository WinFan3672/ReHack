using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Programs.SSH;
using ReHack.Exceptions;

namespace ReHack.Programs.Sudo
{
	/// <summary>Runs commands as root.</summary>
	public static class Sudo
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 0)
			{
				Console.WriteLine("usage: sudo [command]");
				return false;
			}

			if (!RunningUser.CanSudo)
			{
				throw new ErrorMessageException("This user is disallowed from running sudo");
			}

			if (RunningUser.Privileged)
			{
				throw new ErrorMessageException("User is already privileged");
			}

			Console.WriteLine($"[sudo] password for {RunningUser.Username}: ");
			string Password = Console.ReadLine() ?? throw new EndOfStreamException();
			if (Password != RunningUser.Password)
			{ 
				throw new ErrorMessageException("Invalid password");
			}

			string Command = string.Join(" ", Args);
			User SudoUser = new User("root", null, true, false);
			SSHClient.RunCommand(Client, Command, SudoUser);
			return true;
		}
	}
}
