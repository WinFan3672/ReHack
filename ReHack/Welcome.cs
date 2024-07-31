using ReHack.Node;
using ReHack.Node.MailServer;
using ReHack.Node.Player;
using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Programs.SSH;
using ReHack.Data.Nodes;
using ReHack.Filesystem;

namespace ReHack.Welcome {
	public static class WelcomeSequence
	{
		public static bool Init()
		{
			///
			/// <summary>
			/// Starts the game by creating a player node and SSH'ing into it without authenticating.
			/// </summary>

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
			NodeData.Init();
			User PlayerUser = Player.GetUser(Details.Item1);
			SSHClient.ServiceRunner(Player, PlayerUser, !DebugUtils.IsDebug(), false);

			return false;
		}
	}
}
