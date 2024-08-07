using ReHack.Node.Webserver;
using ReHack.Networks;
using ReHack.BaseMethods;
using ReHack.WebRendering;
using Spectre.Console;
using System.Xml;

namespace ReHack.Node.News
{
	/// <summary>An individual news article</summary>
	public class NewsArticle
	{
		/// <summary>Article title</summary>
		public string Title {get; set; }
		/// <summary>Article author</summary>
		public string Author {get; set; }
		/// <summary>Article content</summary>
		public string Content {get; set; }

		/// <summary>Node UID for article</summary>
		public string UID {get; set; }

		/// <summary>Constructor.</summary>
		public NewsArticle(string Title, string Author, string Content, string UID)
		{
			this.Title = Title;
			this.Author = Author;
			this.Content = Content;
			this.UID = UID;
		}

	}
	/// <summary>A news server serves news.</summary>
	public class NewsServer : WebServer
	{
		/// <summary>News articles associated with the node.</summary>
		public List<NewsArticle> Articles {get; set; }

		/// <summary>Constructor.</summary>
		public NewsServer(string Name, string UID, string Address, AreaNetwork? Network) : base(Name, UID, Address, Network, "") 
		{
			Articles = new List<NewsArticle>();
		}

		/// <summary>Adds an article.</summary>
		public void AddArticle(NewsArticle Article)
		{
			Articles.Add(Article);
		}

		/// <summary>Gets a list of articles as a dict in the format {title: article}</summary>
		public Dictionary<string, NewsArticle> GetArticlesAsDic()
		{
			Dictionary<string, NewsArticle> Dic = new Dictionary<string, NewsArticle>();
			foreach(NewsArticle Article in Articles)
			{
				Dic[Article.Title] = Article;
			}
			return Dic;
		}

		/// <summary>Renders an article</summary>
		public string RenderArticle(NewsArticle Article)
		{
			return WebRender.GenerateBasicWebpage(Article.Title, Article.Content, $"(c) {Name}, all rights reserved");
		}

		/// <summary>Renders server</summary>
		public override void Render(BaseNode Client)
		{
			Console.Clear();
			if (!CheckAccessControl(Client))
			{
				RenderAccessDenied();
				return;
			}
			
			Dictionary<string, NewsArticle> Articles = GetArticlesAsDic();
			if (Articles.Keys.Count == 0)
			{
				WebRender.Render(WebRender.GenerateBasicWebpage("Error 500", "This news server has no content to serve.", $"(c) {Name}, all rights reserved", true));
				return;
			}
			string ArticleChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(Name).AddChoices(Articles.Keys));
			Console.Clear();
			WebRender.Render(RenderArticle(Articles[ArticleChoice]));
		}
	}

	/// <summary>Tools for loading news article</summary>
	public static class NewsUtils
	{
		/// <summary>Gets the content of an article by a filename</summary>
		public static string GetArticleText(string filename)
		{
			string Content = FileUtils.GetFileContents($"News.Articles.{filename}.txt") ?? throw new ArgumentException();
			return Content.TrimEnd('\n') ?? throw new ArgumentException(filename);
		}
		/// <summary>Takes news article XML data and returns a NewsArticle from it.</summary>
		public static NewsArticle GenerateArticle(XmlNode Root, string UID)
		{
			XmlNode TitleNode = Root.SelectSingleNode("//Title") ?? throw new XmlException("No title");
			XmlNode AuthorNode = Root.SelectSingleNode("//Author") ?? throw new XmlException("No author");
			XmlNode TextNode = Root.SelectSingleNode("//Text") ?? throw new XmlException("No text");
			return new NewsArticle(TitleNode.InnerText, AuthorNode.InnerText, GetArticleText(TextNode.InnerText), UID);
		}

		/// <summary>Returns the UID associated with an article</summary>
		public static string GetID(XmlNode Root)
		{
			XmlAttributeCollection Attributes = Root.Attributes ?? throw new XmlException("No attributes in root tag");
			XmlAttribute ID = Attributes["id"] ?? throw new XmlException("No id attribute in root tag");
			return ID.Value;
		}

		/// <summary>Returns the article's Unique ID</summary>
		public static string GetArticleID(XmlNode Root)
		{
			XmlNode Tag = Root.SelectSingleNode("//ArticleID") ?? throw new XmlException("Missing ID tag for article");
			return Tag.InnerText ?? throw new XmlException($"Missing ID value for article");
		}

		/// <summary>Adds articles from an XML data file</summary>
		public static void AddArticleFromFile(string FileName)
		{
			XmlDocument Doc = new XmlDocument();
			Doc.LoadXml(FileUtils.GetFileContents($"News.{FileName}.xml") ?? throw new ArgumentException());
			Dictionary<string, NewsArticle> Articles = new Dictionary<string, NewsArticle>();
			foreach(XmlNode Node in Doc.SelectNodes("//Articles/Article") ?? throw new XmlException("No articles in file"))
			{
				Articles[GetArticleID(Node)] = GenerateArticle(Node, GetID(Node));
			}

			foreach (KeyValuePair<string, NewsArticle> Article in Articles)
			{
				NewsServer Target = NodeUtils.GetNode(Article.Value.UID) as NewsServer ?? throw new ArgumentException($"Invalid ID '{Article.Key}' for news article '{FileName}.xml'");
				Target.AddArticle(Article.Value);
			}
		}
	}
}
