using ReHack.Node.Webserver;
using ReHack.Networks;
using Spectre.Console;
using ReHack.BaseMethods;

namespace ReHack.Node.Wiki
{
	/// <summary>A wiki page base class.</summary>
	public interface IWikiPage
	{
		/// <summary>Page title</summary>
		string Title {get; set; }
		/// <summary>Render function - called when selecting the page</summary>
		void Render(BaseNode Client);
	}

	/// <summary>A wiki page</summary>
	public class WikiPage : IWikiPage
	{
		///
		public string Title {get; set; }
		///
		public string Content {get; set; }

		///
		public WikiPage(string Title, string Content)
		{
			this.Title = Title;
			this.Content = Content;
		}

		///
		public void Render(BaseNode Client)
		{
			Console.Clear();
		}
	}

	/// <summary>Wiki category - contains pages</summary>
	public class WikiCategory : IWikiPage
	{
		///
		public string Title {get; set; }
		/// <summary>Unique ID for the category</summary>
		public string UID {get; set; }
		///
		public List<IWikiPage> Pages {get; set; }

		///
		public WikiCategory(string Title, string UID)
		{
			this.Title = Title;
			this.UID = UID;
			Pages = new List<IWikiPage>();
		}

		/// <summary>Returns a page list</summary>
		public Dictionary<string, IWikiPage> ListPages(bool AddExit = false)
		{
			Dictionary<string, IWikiPage> Pages = new Dictionary<string, IWikiPage>();
			foreach(IWikiPage Page in this.Pages)
			{
				Pages[Page.Title] = Page;
			}
			if (AddExit)
			{
				Pages["Exit"] = new WikiPage("Exit", "This is not a real page");
			}
			return Pages;
		}		

		///
		public void Render(BaseNode Client)
		{
			bool Running = true;
			string Selection;
			while (Running)
			{
				Selection = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(Title).AddChoices(ListPages(true).Keys));
				if (Selection == "Exit")
				{
					Running = false;
				}
				else
				{
					ListPages()[Selection].Render(Client);
				}
			}
		}

		/// <summary>Adds a new category</summary>
		public WikiCategory AddCategory(string Title)
		{
			WikiCategory Cat = new WikiCategory(Title, UID);
			Pages.Add(Cat);
			return Cat;
		}

		/// <summary>Loads a page from a file.</summary>
		public void LoadPage(string Title, string FileName)
		{
			string PageContent = FileUtils.GetFileContents($"Wiki.{UID}.{FileName}.txt") ?? throw new ArgumentException("Invalid file");
			Pages.Add(new WikiPage(Title, PageContent));
		}
	}

	/// <summary>Wiki server</summary>
	public class WikiServer : WebServer
	{
		/// <summary>The root category</summary>
		public WikiCategory RootPage {get; set; }
		///
		public WikiServer(string Name, string UID, string Address, AreaNetwork? Network, string? AdminPassword) : base(Name, UID, Address, Network, "", AdminPassword)
		{
			RootPage = new WikiCategory(Name, UID);
		}

		///
		public override void Render(BaseNode Client)
		{
			if (!CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}
			
			RootPage.Render(Client);
		}
	}
}
