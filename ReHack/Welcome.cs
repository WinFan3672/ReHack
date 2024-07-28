using ReHack.Node;
using ReHack.Node.MailServer;
using ReHack.Node.Player;
using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Programs.SSHClient;

namespace ReHack.Welcome {
	public static class WelcomeSequence
	{
		public static void Init()
		{
			///
			/// <summary>
			/// Starts the game by creating a player node and SSH'ing into it without authenticating.
			/// </summary>
			
			if (Console.IsOutputRedirected)
			{
				Console.WriteLine("ERROR: ReHack must be running in a terminal.");
				return;
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
			User PlayerUser = Player.GetUser(Details.Item1);
			SSHClient.ServiceRunner(Player, PlayerUser, !DebugUtils.IsDebug(), false);
		}
	}
}
