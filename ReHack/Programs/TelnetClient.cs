using ReHack.Data;
using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Programs.SSHClient;

namespace ReHack.Programs.TelnetClient
{
    public static class TelnetClient
    {
		private static void ServiceRunner(BaseNode Client, User Person)
		{
			/*SSHClient.Program(Client, Person, false, false);*/
		}
        public static bool Program(BaseNode Client)
        {
            if (NodeUtils.CheckPort(Client, "telnet"))
            {
				(string, string) Details = UserUtils.GetCredentials();
				User Person = Client.GetUser(Details.Item1);
				if (Person.Password != Details.Item2)
				{
					Console.WriteLine("ERROR: Incorrect password.");
					return false;
				}
				ServiceRunner(Client, Person);
                return true;
            }
            return false;
        }
    }
}
