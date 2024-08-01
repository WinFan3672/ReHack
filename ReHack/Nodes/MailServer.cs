using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Node.Webserver;

namespace ReHack.Node.Mail
{
    public class MailAccount {
        public string Username {get; set; }
        public string Address {get; set; }
        public string? Password {get; set; }
        public Email[] Inbox {get; }
        
        private int EmailIndex = 0;

        public MailAccount(string Username, string Address, string? Password) {
            this.Username = Username; 
            this.Address = Address;
            this.Password = Password;
            this.Inbox = new Email[1024];
        }
        
        public bool ReceiveEmail(Email Mail)
        {
            if (this.EmailIndex < this.Inbox.Length)
            {
                this.EmailIndex++;
                this.Inbox[this.EmailIndex] = Mail;
                return true;
            }
            else {
                return false;
            }
        }

    }

    public class MailServer : WebServer {
        public List<MailAccount> Accounts {get; } = new List<MailAccount>();
		public bool AllowLookup {get; } = true;
        public MailServer(string Name, string UID, string Address, string? AdminPassword=null, string IndexFolder="MailServer") : base(Name, UID, Address, IndexFolder, AdminPassword) {
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

		public virtual (List<string>, bool) Lookup(BaseNode Client)
		{
			if (!CheckAccessControl(Client) || !AllowLookup)
			{
				return (new List<string>(), false);
			}
			else
			{
				return (ListAccounts(), true);
			}
		}
    }
}
