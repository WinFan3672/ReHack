using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Programs.MOTD
{
	/// <summary>Prints the MOTD.</summary>
	public static class MotdClient
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			Client.Welcome();
			return true;
		}
	}
}
