namespace ReHack.Missions
{
	public interface IMissionCheck
	{
		public bool Check();
	}

	public class Mission
	{
		public string Name {get; set; }
		public string ID {get; set; }
		public int Payment {get; set; }
		public List<IMissionCheck> Checks {get; } = new List<IMissionCheck>();
		
		public Mission(string Name, string ID, int Payment)
		{
			this.Name = Name;
			this.ID = ID;
			this.Payment = Payment;
		}

		public void AddCheck(IMissionCheck Check)
		{
			this.Checks.Add(Check);
		}
	}
}
