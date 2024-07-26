using ReHack.BaseMethods;
using ReHack.Node;

namespace ReHack.Node.Player
{
    public class PlayerNode : BaseNode {
        public PlayerNode(string Name, string Password) : base(Name, "localhost", "127.0.0.1", new[] {new User(Name, Password, true)}) {
        }
    }
}
