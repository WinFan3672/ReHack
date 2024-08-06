namespace ReHack.Missions
{
	/// <summary>A mission check template</summary>
	public interface IMissionCheck
	{
		/// <summary>Returns true if the mission check is completed</summary>
		public bool Check();
	}

	/// <summary>A mission</summary>
	public class Mission
	{
		///
		public string Name {get; set; }
		/// <summary>ID used to grab the mission in code.</summary>
		public string ID {get; set; }
		/// <summary>How much the mission pays.</summary>
		public int Payment {get; set; }
		/// <summary>A list of 'checks' (or objectives, if you like) the mission needs to pass to be completed</summary>
		public List<IMissionCheck> Checks {get; } = new List<IMissionCheck>();
	
		///
		public Mission(string Name, string ID, int Payment)
		{
			this.Name = Name;
			this.ID = ID;
			this.Payment = Payment;
		}

		/// <summary>Adds a mission check</summary>
		public void AddCheck(IMissionCheck Check)
		{
			this.Checks.Add(Check);
		}
	}
}
