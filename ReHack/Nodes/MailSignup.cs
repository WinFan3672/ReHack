using ReHack.Node.Mail;
using ReHack.Node.Webserver;
using ReHack.BaseMethods;
using ReHack.WebRendering;
using ReHack.Networks;

namespace ReHack.Node.MailSignup
{
	public class MailSignupService : WebServer
	{
		public string WelcomeMessage {get;} = "Thank you for creating an account with us.";
		public MailSignupService(string Name, string UID, string Address, AreaNetwork? Network, string Target, string? AdminPassword=null) : base (Name, UID, Address, Network, Target, AdminPassword)
		{
		}

		public override void Render(BaseNode Client, string Resource="/")
		{
			if (!CheckAccessControl(Client))
			{
				WebRender.Render("<Webpage><Head><Title>Error 403</Title></Head><Body><Title /><Text>Access to this website is denied.</Text></Body></Webpage>");
				return;
			}

			MailServer? Target = NodeUtils.GetNode(this.IndexFolder) as MailServer ?? throw new ArgumentException("Invalid mail server");

			Console.Write("Username $");
			string Username = Console.ReadLine() ?? throw new EndOfStreamException();
			if (Target.ListAccounts().Contains(Username))
			{
				WebRender.Render("<Webpage><Head><Title>Error</Title></Head><Body><Text>[bold red]error[/]: This username is already taken.</Text></Body></Webpage>");
				return;
			}

			Console.Write("Password $");
			string Password = PrintUtils.ReadPassword();
			if (Password == "")
			{
				WebRender.Render("<Webpage><Head><Title>Error</Title></Head><Body><Text>[bold red]error[/]: Empty passwords are not acceptable.</Text></Body></Webpage>");
				return;
			}

			MailAccount Account = new MailAccount(Username, Target.Address, Password);
			Target.Accounts.Add(Account);
			Console.WriteLine(WelcomeMessage);
		}

	}
}
