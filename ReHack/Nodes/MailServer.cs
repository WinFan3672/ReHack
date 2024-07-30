using ReHack.BaseMethods;
using ReHack.Data;

namespace ReHack.Node.MailServer
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

    public class MailServer : BaseNode {
        public List<MailAccount> Accounts {get; } = new List<MailAccount>();
        public MailServer(string Name, string UID, string Address, string? AdminPassword=null) : base(Name, UID, Address, new User[] { new User("root", null, true) }) {
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
    }
}
