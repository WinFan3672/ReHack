using ReHack.Data;
using ReHack.Node;

namespace ReHack.BaseMethods
{
    public static class NodeUtils
    {
        private static readonly Random Rand = new Random();

        public static string GenerateIPAddress() {
            /// <summary>
            /// Generates a random IP address
            /// </summary>
            string Address =  $"{Rand.Next(0, 256)}.{Rand.Next(0, 256)}.{Rand.Next(0, 256)}.{Rand.Next(0, 256)}";
            foreach (var Node in GameData.Nodes)
            {
                if (Node.Address == Address)
                {
                    return GenerateIPAddress();
                }
            }
            return Address;
        }

        public static BaseNode GetNode(string UID)
        {
            foreach(var Node in GameData.Nodes)
            {
                if (Node.UID == UID)
                {
                    return Node;
                }
            }
            throw new Exception("Invalid node");
        }
    }

    public class User {
        /// <summary>
        /// User class - each Node has an array of User objects which can be logged into.
        /// </summary>
        public string Username {get; set; }
        public string? Password {get; set; }
        public bool Privileged {get; set; } // If this is enabled, the user can perform privileged operations

        public User(string Username, string Password, bool Privileged)
        {
            this.Username = Username;
            this.Password = Password;
            this.Privileged = Privileged;
        }
    }

    public static class UserUtils
    {
        public static string PickPassword() {
            /// <summary>
            /// Returns a random password that the game *can* brute-force.
            /// </summary>
            return "root"; // Just a stub
        }
    }

    public class Email {
        /// <summary>
        /// Email class - defines a recipient, sender, subject, and content
        /// </summary>
        public string Recipient {get; set; }
        public string Sender {get; set; }
        public string Subject {get; set; }
        public string Content {get; set; }
        // public Link[] Links {get; set; }

        public Email(string Recipient, string Sender, string Subject, string Content) {
            this.Recipient = Recipient;
            this.Sender = Sender;
            this.Subject = Subject;
            this.Content = Content;
        }

        public bool Send()
        {
            return false;
        }
    }

    public class Port {
        /// <summary>
        /// Port - Has a service name and ID, port number, and can be open/closed.
        /// </summary>
        public string ServiceName {get; set; }
        public string ServiceID {get; set; }
        public int PortNumber {get; set; }
        public bool Open {get; set;}

        public Port(string ServiceName, string ServiceID, int PortNumber, bool Open=false)
        {
            this.ServiceName = ServiceName;
            this.ServiceID = ServiceID;
            this.PortNumber = PortNumber;
            this.Open = Open;
        }
    }

    public static class PrintUtils
    {
        public static void Divider()
        {
            Console.WriteLine("--------------------");
        }
    }
}
