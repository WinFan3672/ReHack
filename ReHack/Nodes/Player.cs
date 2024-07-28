using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Data;

namespace ReHack.Node.Player
{
    public class PlayerNode : BaseNode {
        public PlayerNode(string Username, string Password) : base("Localhost", "localhost", "127.0.0.1", new[] {new User(Username, Password, true)}) {
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("rehack"));
			if (DebugUtils.IsDebug())
			{
				this.AddProgram("debug");
			}
        }
    }
}
