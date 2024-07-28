using ReHack.BaseMethods;

namespace ReHack.Node {
    public class BaseNode {
        public string Name {get; set; }
        public string UID {get; set; }
        public string Address {get; set; }
        public User[] Users{get; set; }
        public List<Port> Ports {get; }
        public Dictionary<string, bool> Programs {get; }

        public BaseNode(string Name, string UID, string Address, User[] Users)
        {
            this.Name = Name;
            this.UID = UID;
            this.Address = Address;
            this.Users = Users;
            this.Ports = new List<Port>();
            this.Programs = new Dictionary<string, bool>();
        }

        public Port GetPort(string PortID)
        {
            /// <summary>
            /// Returns a port in the port list
            /// </summary>
            foreach (Port Service in this.Ports)
            {
                if (Service.ServiceID == PortID)
                {
                    return Service;
                }
            }
            throw new Exception("Invalid port");
        }

        public void DebugPorts()
        {
            foreach (Port Service in this.Ports)
            {
                PrintUtils.Divider();
                Console.WriteLine($"Port list for {this.Name}");
                PrintUtils.Divider();
                Console.WriteLine($"{Service.ServiceID} = {Service.PortNumber}");
                PrintUtils.Divider();
            }
        }

		public User GetUser(string Username)
		{
			foreach (User Person in this.Users)
			{
				if (Username == Person.Username)
				{
					return Person;
				}
			}
			throw new Exception("Invalid username");
		}
    }
}
