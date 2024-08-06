using ReHack.BaseMethods;
using ReHack.Networks;
using ReHack.Data;

namespace ReHack.Node.Bank
{
	/// <summary>A bank node is designed to hold a large quantity of cash.</summary>
	public class BankNode : BaseNode
	{
		/// <summary>Constructor</summary>
		public BankNode(string Name, string UID, string Address, string? AdminPassword, AreaNetwork? Network, int Balance=0) : base(Name, UID, Address, new User[] {new User("root", AdminPassword, true, false),}, Network)
		{
			this.Balance = Balance;
			this.Ports.Add(GameData.GetPort("ssh"));
		}
	}
}
