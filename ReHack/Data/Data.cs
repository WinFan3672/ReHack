using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Node.MailServer;

namespace ReHack.Data
{
    public static class GameData {
        public static List<BaseNode> Nodes { get; } = new List<BaseNode>();

        public static List<Port> Ports {get; } = new List<Port> { 
            new Port("Local Area Network Router", "lan", 1),
            new Port("FTP", "ftp", 21),
            new Port("SSH", "ssh", 22),
            new Port("Mail Server (SMTP)", "smtp", 25),
            new Port("Telnet", "telnet", 23),
            new Port("Domain Name Service", "dns", 53),
            new Port("HTTP Server", "http", 80),
            new Port("Usenet (NNTP) Server", "nntp", 119),
            new Port("NTP Time Server", "ntp", 123),
            new Port("OpenVPN Server", "vpn", 1194),
            new Port("SQL Database", "sql", 1433),
            new Port("IRC Server", "irc", 6667),
            new Port("BitTorrent", "torrent", 6881),
            new Port("ReHackOS Node", "rehack", 7777),
            new Port("BlueMedical Monitor Service", "medical", 8989),
            new Port("Tor Relay", "tor", 9200),
        };

        public static BaseNode AddNode(BaseNode Node)
        {
            Nodes.Add(Node);
            return Node;
        }

        public static Port GetPort(string PortID)
        {
            foreach (Port Service in Ports)
            {
                if (Service.ServiceID == PortID)
                {
                    return Service;
                }
            }
            throw new Exception("Invalid port");
        }

        public static void AddPort(BaseNode Node, string PortID)
        {
            Port P = GetPort(PortID);
            Node.Ports.Add(P);
        }

        public static void DebugNodes()
        {
            PrintUtils.Divider();
            Console.WriteLine("Node List");
            PrintUtils.Divider();
            foreach (var Node in Nodes)
            {
                Console.WriteLine($"{Node.UID} = {Node.Name} = {Node.Address}");
            }
            PrintUtils.Divider();
        }

        public static List<string> BannedUsernames = new List<string> { "admin", "root" };
    }
}
