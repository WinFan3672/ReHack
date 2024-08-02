using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Programs.MOTD
{
	public static class MotdClient
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			Client.Welcome();
			return true;
		}
	}
}
