using ReHack.BaseMethods;
using ReHack.Exceptions;
using ReHack.Node;
using ReHack.Programs.SSH;
using Spectre.Console;

namespace ReHack.Programs.Telnet
{
    public static class TelnetClient
    {
		private static void ServiceRunner(BaseNode Client, User Person)
		{
			SSHClient.ServiceRunner(Client, Person, false, false);
		}
        public static bool Program(string[] Args, BaseNode Client, User RunningUser)
        {
			if (Args.Length == 1)
            {
				BaseNode Target = NodeUtils.GetNodeByAddress(Args[0]);
				if (!NodeUtils.CheckPort(Target, "telnet"))
				{
					AnsiConsole.MarkupLine("[bold red]error[/]: Connection refused");
					return false;
				}
				Console.Write("Username $");
				string Username = Console.ReadLine() ?? throw new EndOfStreamException();
				Console.Write("Password $");
				string Password = PrintUtils.ReadPassword(false);
				User Person;
				try
				{
					Person = Target.GetUser(Username);
				}
				catch (ArgumentException ex)
				{
					throw new ErrorMessageException(ex.Message);
				}
				if (Person.Password != Password)
				{
					throw new ErrorMessageException("Incorrect password");
				}
				ServiceRunner(Target, Person);
                return true;
            }
			else
			{
				AnsiConsole.MarkupLine("[bold blue]usage[/]: telnet [[hostname]]");
				return false;
			}
        }
    }
}
