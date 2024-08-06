using ReHack.BaseMethods;
using ReHack.Node;

namespace ReHack.Missions
{
	/// <summary>A mission check that confirms that a node is dead.</summary>
	public class NodeDeadCheck : IMissionCheck
	{
		
		/// <summary>Target node UID</summary>
		public string UID {get; set; }

		///
		public NodeDeadCheck(string UID)
		{
			this.UID = UID;
		}

		///
		public bool Check()
		{
			BaseNode Node = NodeUtils.GetNode(UID);
			return !Node.CheckHealth();
		}
	}

	/// <summary>A mission check that confirms that a port has been opened on a node.</summary>
	public class PortOpenCheck : IMissionCheck
	{
		/// <summary>Target node UID</summary>
		public string NodeUID {get; set; }
		/// <summary>Target port ID</summary>
		public string ServiceID {get; set; }

		///
		public PortOpenCheck(string NodeUID, string ServiceID)
		{
			this.NodeUID = NodeUID;
			this.ServiceID = ServiceID;
		}

		///
		public bool Check()
		{
			BaseNode Node = NodeUtils.GetNode(NodeUID) ?? throw new ArgumentException("Invalid check node");
			return NodeUtils.CheckPort(Node, ServiceID);
		}
	}
}
