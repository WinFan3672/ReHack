using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Programs.SSHClient;

namespace ReHack.Programs.Sudo
{
	public static class Sudo
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (Args.Length == 0)
			{
				Console.WriteLine("usage: sudo [command]");
				return false;
			}

			if (RunningUser.Privileged)
			{
				Console.WriteLine("error: User is already privileged.");
				return false;
			}

			string Command = string.Join(" ", Args);
			User SudoUser = new User("root", null, true);
			/*SSHClient.RunCommand(Client, Command, SudoUser);*/
			return true;
		}
	}
}
