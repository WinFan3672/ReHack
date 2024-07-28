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
			var Details = UserUtils.GetCredentials();
			PlayerNode Player = new PlayerNode(Details.Item1, Details.Item2);
			User PlayerUser = Player.GetUser(Details.Item1);
			SSHClient.Program(Player, PlayerUser, true, false);
		}
	}
}
