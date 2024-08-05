using ReHack.BaseMethods;
using ReHack.Networks;
using ReHack.Data;

namespace ReHack.Node.Bank
{
	public class BankNode : BaseNode
	{
		public BankNode(string Name, string UID, string Address, string? AdminPassword, AreaNetwork? Network, int Balance=0) : base(Name, UID, Address, new User[] {new User("root", AdminPassword, true, false),}, Network)
		{
			this.Balance = Balance;
			this.Ports.Add(GameData.GetPort("ssh"));
		}
	}
}
