using ReHack.BaseMethods;
using ReHack.Data;

namespace ReHack.Node.MailServer
{
    public class MailAccount {
        public string Username {get; set; }
        public string Address {get; set; }
        public string Password {get; set; }
        public Email[] Inbox {get; }
        
        private int EmailIndex = 0;

        public MailAccount(string Username, string Address, string Password) {
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
        private int AccountIndex = 0;

        public MailAccount[] Accounts {get; }
        public MailServer(string Name, string UID, string Address) : base(Name, UID, Address, new User[] { new User("root", null, true) }) {
            this.Accounts = new MailAccount[1024];
            this.Ports.Add(GameData.GetPort("smtp"));
        }

        public MailAccount CreateMailAccount(string Username, string Password)
        {
            MailAccount Account = new MailAccount(Username, this.Address, Password);
            if (this.AccountIndex < this.Accounts.Length)
            {
                Accounts[this.AccountIndex] = Account;
                this.AccountIndex++;
                return Account;
            }
            else {
                throw new Exception("Too many accounts");
            }
        }
    }
}
