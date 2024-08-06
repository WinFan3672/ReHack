using ReHack.BaseMethods;
using ReHack.Data;
using ReHack.Node.Webserver;
using ReHack.Networks;

namespace ReHack.Node.Mail
{
	/// <summary>An account on a mail server.</summary>
	public class MailAccount {
		/// <summary>The username to sign into.</summary>
		public string Username {get; set; }
		/// <summary>The address of the account (i.e the bit after the @)</summary>
		public string Address {get; set; }
		/// <summary>Account password.</summary>
		public string? Password {get; set; }
		/// <summary>Email inbox.</summary>
		public List<Email> Inbox {get; set; }

		/// <summary>Constructor.</summary>
		public MailAccount(string Username, string Address, string? Password) {
			this.Username = Username; 
			this.Address = Address;
			this.Password = Password;
			this.Inbox = new List<Email>();
		}
	}

	/// <summary>Mail server. Can have a webpage goin as well, defaults to a basic 'this is a mail server' page.</summary>
	public class MailServer : WebServer {
		/// <summary>Mail accounts associated with the server</summary>
		public List<MailAccount> Accounts {get; } = new List<MailAccount>();
		/// <summary>Whether or not to allow looking up the email address list.</summary>
		public bool AllowLookup {get; set; } = true;

		/// <summary>Constructor.</summary>
		public MailServer(string Name, string UID, string Address, AreaNetwork? Network, string? AdminPassword=null, string IndexFolder="MailServer") : base(Name, UID, Address, Network, IndexFolder, AdminPassword) {
			AddPort("smtp");
			CreateMailAccount("admin", AdminPassword);
		}

		/// <summary>Creates a new mail account</summary>
		public MailAccount CreateMailAccount(string Username, string? Password)
		{
			MailAccount Account = new MailAccount(Username, Address, Password);
			this.Accounts.Add(Account);
			return Account;
		}

		/// <summary>Debugging function.</summary>
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

		/// <summary>Returns a list of email accounts.</summary>
		public List<string> ListAccounts()
		{
			List<string> Accounts = new List<string>();
			foreach(MailAccount Account in this.Accounts)
			{
				Accounts.Add(Account.Username);
			}
			return Accounts;
		}

		/// <summary>Performs an email address lookup.</summary>
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

		/// <summary>Retrieves an account from a username.</summary>
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
