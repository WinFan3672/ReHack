using ReHack.Node;
using ReHack.BaseMethods;

namespace ReHack.Networks
{
	public class AreaNetwork
	{
		public List<BaseNode> Nodes {get;} = new List<BaseNode>();

		public AreaNetwork()
		{}

		public string GenerateIPAddress()
		{
			/// <summary>
			/// Returns a 10.x.y.z range IP address usable in the network.
			/// </summary>
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
