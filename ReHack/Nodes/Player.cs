using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Data;
using ReHack.Filesystem;

namespace ReHack.Node.Player
{
    public class PlayerNode : BaseNode {
        public PlayerNode(string Username, string Password) : base("Localhost", "localhost", "127.0.0.1", new[] {new User(Username, Password, true)}) {
			// Add ports
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("rehack"));
			
			// Add debug
			if (DebugUtils.IsDebug())
			{
				this.AddProgram("debug");
			}

			// Add ReHack repo
			VirtualFile AptConfig = this.Root.GetFile("/etc/apt/sources.list");
			AptConfig.Content = AptConfig.Content + "\npkg.rehack.org";
        }
    }
}
