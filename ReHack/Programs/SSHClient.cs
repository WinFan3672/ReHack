using ReHack.Data;
using ReHack.BaseMethods;
using ReHack.Node;

namespace ReHack.Programs.SSHClient
{
    public static class SSHClient
    {
		private static bool Authenticate(BaseNode Client, string Username)
		{
			User Person = Client.GetUser(Username);
			Console.Write($"ssh: {Username}@{Client.Address}'s password: ");
			string Password = PrintUtils.ReadPassword();
			return Password == Person.Password;
		}
        private static void ServiceRunner(BaseNode Client, User Person, bool ConfirmExit=false, bool DoAuthenticate = true)
        {
			if (DoAuthenticate)
			{
				if (!Authenticate(Client, Person.Username))
				{
					Console.WriteLine("Permission denied (password).");
					return;
				}
			}
            string Input;
            while (true)
            {
                Console.Write($"{Person.Username}@{Client.Address} $");
                Input = Console.ReadLine();
                if (Input == "exit" || Input == "quit")
                {
					if (!ConfirmExit || PrintUtils.Confirm("Are you sure you want to exit?", false))
					{
						return;
					}
                }
                else {
                }
            }
        }
        public static bool Program(BaseNode Client, User Person, bool ConfirmExit=false, bool DoAuthenticate=true)
        {
            if (NodeUtils.CheckPort(Client, "ssh"))
            {
                ServiceRunner(Client, Person, ConfirmExit, DoAuthenticate);
                return true;
            }
            return false;
        }
    }
}
