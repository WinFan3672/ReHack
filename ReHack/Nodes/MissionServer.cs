using ReHack.Node.Webserver;
using ReHack.Missions;
using ReHack.Networks;

namespace ReHack.Node.MissionServer
{
	public class MissionServer : WebServer
	{
		public List<Mission> Missions {get; set; }

		public MissionServer(string Name, string UID, string Address, List<Mission> Missions, string? AdminPassword, AreaNetwork? Network) : base (Name, UID, Address, Network, "", AdminPassword)
		{
			this.Missions = Missions;
			AddPort("sql");
		}

		public override void Render(BaseNode Client)
		{
			if (!CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}
		}
	}
}
