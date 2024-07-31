using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Missions;

namespace ReHack.Node.MissionServer
{
	public class MissionServer : BaseNode
	{
		public List<Mission> Missions {get; set; }

		public MissionServer(string Name, string UID, string Address, List<Mission> Missions) : base (Name, UID, Address, new User[] { new User("root", null, true) })
		{
			this.Missions = Missions;
			AddPort("sql");
		}
	}
}
