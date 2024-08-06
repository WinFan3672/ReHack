using ReHack.Node.Player;
using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Programs.SSH;
using ReHack.Data.Nodes;

namespace ReHack.Welcome {
	/// <summary>The welcome sequence referrs to what happens as soon as you hit 'New Game'.</summary>
	public static class WelcomeSequence
	{
		/// <summary>Starts the game.</summary>
		public static bool Init()
		{
			if (Console.IsOutputRedirected)
			{
				Console.WriteLine("ERROR: ReHack must be running in a terminal.");
				return false;
			}

			(string, string) Details;

			if (DebugUtils.IsDebug())
			{
				Details = ("gordinator", "root");
			}
			else
			{
				Details = UserUtils.GetCredentials();
			}
			PlayerNode Player = new PlayerNode(Details.Item1, Details.Item2);
			GameData.AddNode(Player);
			NodeData.Init(Player);
			User PlayerUser = Player.GetUser(Details.Item1);

			PerformTransactions();

			SSHClient.ServiceRunner(Player, PlayerUser, !DebugUtils.IsDebug(), false);
			return false;
		}

		private static void PerformTransactions()
		{
			BankUtils.PerformTransaction(new BankTransaction("bank.rehack.org", "127.0.0.1", 500, "Signup bonus"));
		}
	}
}
