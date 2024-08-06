using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Networks
{
	/// <summary>A network of computers.</summary>
	public class AreaNetwork
	{
		/// <summary>The nodes in the network.</summary>
		public List<BaseNode> Nodes {get; set; }

		/// <summary>Constructor.</summary>
		public AreaNetwork()
		{
			Nodes = new List<BaseNode>();
		}

		/// <summary>
		/// Returns a 10.x.y.z range IP address usable in the network.
		/// </summary>
		public string GenerateIPAddress()
		{
			string Address = NodeUtils.GenerateIPAddress(true);
			foreach (BaseNode Node in this.Nodes)
			{
				if (Node.Address == Address)
				{
					return this.GenerateIPAddress();
				}
			}
			return Address;
		}

		/// <summary>Returns a node in the network.</summary>
		public BaseNode GetNode(string UID)
		{
			foreach(BaseNode Node in this.Nodes)
			{
				if (Node.UID == UID)
				{
					return Node;
				}
			}
			throw new ArgumentException("Invalid node");
		}

		/// <summary>Returns a node in the network from the IP address.</summary>
		public BaseNode GetNodeByAddress(string Address)
		{
			foreach(BaseNode Node in this.Nodes)
			{
				if (Node.Address == Address)
				{
					return Node;
				}
			}
			throw new ArgumentException("Invalid node");
		}
	}
}
