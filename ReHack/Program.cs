namespace ReHack;

using ReHack.Node;
using ReHack.Node.MailServer;
using ReHack.Node.Player;
using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Programs.SSHClient;

class Program
{
    static void Main(string[] args)
    {
        var Test = GameData.AddNode(new BaseNode("Test Node", "test", NodeUtils.GenerateIPAddress(), new[] {new User("root", "toor", true)}));
        Test.Ports.Add(GameData.GetPort("telnet"));

        var JMail = GameData.AddNode(new MailServer("JMail", "jmail", "jmail.com"));
        
        var Details = UserUtils.GetCredentials();

        PlayerNode Player = new PlayerNode(Details.Item1, Details.Item2);
        GameData.AddNode(Player);
        Player.Ports.Add(GameData.GetPort("rehack"));
        Player.Ports.Add(GameData.GetPort("ssh"));

        SSHClient.Program(Player);
    }
}
