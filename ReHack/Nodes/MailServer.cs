using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Node.Webserver;
using ReHack.Networks;

namespace ReHack.Node.Mail
{
    public class MailAccount {
        public string Username {get; set; }
        public string Address {get; set; }
        public string? Password {get; set; }
        public List<Email> Inbox {get; set; }
        
        public MailAccount(string Username, string Address, string? Password) {
            this.Username = Username; 
            this.Address = Address;
            this.Password = Password;
			this.Inbox = new List<Email>();
        }
    }

    public class MailServer : WebServer {
        public List<MailAccount> Accounts {get; } = new List<MailAccount>();
		public bool AllowLookup {get; } = true;
        public MailServer(string Name, string UID, string Address, AreaNetwork? Network, string? AdminPassword=null, string IndexFolder="MailServer") : base(Name, UID, Address, Network, IndexFolder, AdminPassword) {
            this.Ports.Add(GameData.GetPort("smtp"));
            CreateMailAccount("admin", AdminPassword);
        }

        public MailAccount CreateMailAccount(string Username, string? Password)
        {
            MailAccount Account = new MailAccount(Username, this.Address, Password);
            this.Accounts.Add(Account);
            return Account;
        }

        public void DebugAccounts()
        {
            PrintUtils.Divider();
            Console.WriteLine($"Mail Accounts for {this.Name}");
            PrintUtils.Divider();
            foreach (MailAccount Account in this.Accounts)
            {
                Console.WriteLine($"{Account.Username}:{Account.Password}");
            }
            PrintUtils.Divider();
        }

		public List<string> ListAccounts()
		{
			List<string> Accounts = new List<string>();
			foreach(MailAccount Account in this.Accounts)
			{
				Accounts.Add(Account.Username);
			}
			return Accounts;
		}

		public virtual List<string> Lookup(BaseNode Client)
		{
			if (!CheckAccessControl(Client) || !AllowLookup)
			{
				return new List<string>();
			}
			else
			{
				return ListAccounts();
			}
		}

		public MailAccount? GetAccount(string Username)
		{
			foreach(MailAccount Account in Accounts)
			{
				if (Account.Username == Username)
				{
					return Account;
				}
			}
			return null;
		}
    }
}
