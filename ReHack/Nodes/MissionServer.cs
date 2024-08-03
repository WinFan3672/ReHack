using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Missions;
using ReHack.Networks;

namespace ReHack.Node.MissionServer
{
	public class MissionServer : BaseNode
	{
		public List<Mission> Missions {get; set; }

		public MissionServer(string Name, string UID, string Address, List<Mission> Missions, AreaNetwork? Network) : base (Name, UID, Address, new User[] { new User("root", null, true, false) }, Network)
		{
			this.Missions = Missions;
			AddPort("sql");
		}
	}
}
