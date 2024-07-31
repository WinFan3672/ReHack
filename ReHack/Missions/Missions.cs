using ReHack.BaseMethods;
using ReHack.Node;

namespace ReHack.Missions
{
	public interface IMissionCheck
	{
		public bool Check();
	}

	public class Mission
	{
		public string Name {get; set; }
		public string ID {get; set; }
		public List<IMissionCheck> Checks {get; } = new List<IMissionCheck>();
		
		public Mission(string Name, string ID)
		{
			this.Name = Name;
			this.ID = ID;
		}

		public void AddCheck(IMissionCheck Check)
		{
			this.Checks.Add(Check);
		}
	}

	public class PortOpenCheck : IMissionCheck
	{
		public string NodeUID {get; set;}
		public string PortID {get; set; }

		public PortOpenCheck(string NodeUID, string PortID)
		{
			this.NodeUID = NodeUID;
			this.PortID = PortID;
		}

		public bool Check()
		{
			BaseNode Node = NodeUtils.GetNode(NodeUID);
			Port Service = Node.GetPort(PortID);
			return Service.Open;
		}
	}								 
}
