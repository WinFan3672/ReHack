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
		/// <summary>A mission check that confirms that a port has been opened on a node.</summary>
		
		public string NodeUID {get; set; }
		public string ServiceID {get; set; }

		public PortOpenCheck(string NodeUID, string ServiceID)
		{
			this.NodeUID = NodeUID;
			this.ServiceID = ServiceID;
		}

		public bool Check()
		{
			BaseNode Node = NodeUtils.GetNode(NodeUID) ?? throw new ArgumentException("Invalid check node");
			return NodeUtils.CheckPort(Node, ServiceID);
		}
	}
}
