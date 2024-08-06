using ReHack.Node.Webserver;
using ReHack.Missions;
using ReHack.Networks;

namespace ReHack.Node.MissionServer
{
	/// <summary>A server for accepting and completing missions</summary>
	public class MissionServer : WebServer
	{
		/// <summary>The missions the server takes</summary>
		public List<Mission> Missions {get; set; }

		/// <summary>Constructor.</summary>
		public MissionServer(string Name, string UID, string Address, List<Mission> Missions, string? AdminPassword, AreaNetwork? Network) : base (Name, UID, Address, Network, "", AdminPassword)
		{
			this.Missions = Missions;
			AddPort("sql");
		}

		/// <summary>Renders mission server.</summary>
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
