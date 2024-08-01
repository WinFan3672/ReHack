using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.Welcome
{
	public static class WelcomeApp
	{
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			Console.Clear();
			string Option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select Section To Read").AddChoices(new[] {"The Basics"}));
			switch (Option)
			{
				case "The Basics":
					AnsiConsole.MarkupLine($"Hello, [blue]{RunningUser.Username}[/], and welcome to [yellow]ReHack[/]! Please read through this short introduction to understand how to work as an Agent.");
					Console.WriteLine();
					AnsiConsole.MarkupLine("[bold]Basic Commands[/]");
					AnsiConsole.MarkupLine("To use ReHack, you'll need to be at least somewhat familiar with a UNIX command-line. Thankfully, if you're not, this section is for you!");
					AnsiConsole.MarkupLine("The [yellow]ls[/] command lists the contents of [blue]directories[/]. Think of a directory as like a folder. A directory can contain files and subdirectories.");
					AnsiConsole.MarkupLine("Running [yellow]ls[/] on its own shows you the contents of the [blue]root directory[/], whereas specifying a path ([yellow]ls [[path]][/]) shows you the contents of that dorectory.");
					AnsiConsole.MarkupLine("The [yellow]cat[/] command prints out the contents of [blue]files[/]. The usage is [yellow]cat [[filename]][/].");
					AnsiConsole.MarkupLine("The [yellow]apt[/] command is a little more extensive but just as essential. For detailed info on how to use it, run [yellow]man apt[/].");
					AnsiConsole.MarkupLine("The [yellow]w3[/] command lets you view websites. Try it out with [yellow]w3 rehack.org[/].");
					Console.WriteLine();
					AnsiConsole.MarkupLine("[bold]Missions[/]");
					AnsiConsole.MarkupLine("Our mission server ([yellow]jobs.rehack.org[/]) is the primary place you'll accept jobs. Note that [bold]you cannot cancel a mission or start a new one[/], so choose wisely.");
					Console.WriteLine();
					AnsiConsole.MarkupLine("[bold]Email[/]");
					AnsiConsole.MarkupLine($"As part of your onboarding, you'll have received a JMail account (in your case, [yellow]{RunningUser.Username}@jmail.com[/]) where business will be conducted.");
					AnsiConsole.MarkupLine("The [yellow]mail[/] program built into ReHack allows you to view your emails.");
					break;
			}
			
			return true;
		}
	}
}
