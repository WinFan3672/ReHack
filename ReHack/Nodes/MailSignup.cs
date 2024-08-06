using ReHack.Node.Mail;
using ReHack.Node.Webserver;
using ReHack.BaseMethods;
using ReHack.WebRendering;
using ReHack.Networks;

namespace ReHack.Node.MailSignup
{
	/// <summary>A web server that lets users create accounts on a mail server.</summary>
	public class MailSignupService : WebServer
	{
		/// <summary>The message shown to the user when they create an account.</summary>
		public string WelcomeMessage {get;} = "Thank you for creating an account with us.";

		/// <summary>Constructor.</summary>
		public MailSignupService(string Name, string UID, string Address, AreaNetwork? Network, string Target, string? AdminPassword=null) : base (Name, UID, Address, Network, Target, AdminPassword)
		{
		}

		/// <summary>Renders the signup page.</summary>
		public override void Render(BaseNode Client)
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
