namespace ReHack;

using ReHack.Node;
using ReHack.Node.MailServer;
using ReHack.Node.Player;
using ReHack.BaseMethods;
using ReHack.Data;

class Program
{
    static void Main(string[] args)
    {
        var Test = GameData.AddNode(new BaseNode("Test Node", "test", NodeUtils.GenerateIPAddress(), new[] {new User("root", "toor", true)}));
        Test.Ports.Add(GameData.GetPort("telnet"));

        var JMail = GameData.AddNode(new MailServer("JMail", "jmail", "jmail.com"));

        PlayerNode Player = new PlayerNode("gordinator", "root");
        GameData.AddNode(Player);
        Player.Ports.Add(GameData.GetPort("rehack"));
        Player.Ports.Add(GameData.GetPort("ssh"));
    }
}
