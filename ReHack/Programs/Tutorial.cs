using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;

namespace ReHack.Programs.Tutorial
{
	/// <summary>Tutorial program.</summary>
	public static class TutorialApp
	{
		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			Console.Clear();
			AnsiConsole.MarkupLine($"Welcome, [blue]{RunningUser.Username}[/], to the Official ReHack Tutorial!");
			AnsiConsole.MarkupLine($"This tutorial intends to showcase the absolute basics of hacking a system.");
			AnsiConsole.MarkupLine($"As the [yellow]welcome[/] command will have told you, missions will have tasks you need to complete.");
			AnsiConsole.MarkupLine($"Usually, it's some data to steal, an account to create on a node, etc.");
			AnsiConsole.MarkupLine($"In the case of this tutorial, we have a convenient test machine you can very easily break into.");
			Console.WriteLine();
			AnsiConsole.MarkupLine($"[bold]The Target[/]");
			AnsiConsole.MarkupLine($"For the purposes of this test, we will use the ReHack Test Server located at [yellow]test.com[/].");
			AnsiConsole.MarkupLine($"[blue]Exercise[/]: Use [yellow]ping[/] on the ReHack Test Server.");
			Console.WriteLine();
			AnsiConsole.MarkupLine($"[bold]Port Scanning[/]");
			AnsiConsole.MarkupLine($"The first thing you should do on a system is perform reconaissance. The best way to start is using [yellow]nmap[/], a port scanner. It will tell you what services are open on a node.");
			AnsiConsole.MarkupLine($"[blue]Exercise[/]: Perform a port scan on [yellow]test.com[/] using [yellow]nmap[/].");
			Console.WriteLine();
			AnsiConsole.MarkupLine($"[bold]Exploitation Tools[/]");
			AnsiConsole.MarkupLine($"Typically, you'd see what ports are open and decide your method of attack. The test server has Telnet open, which is an ancient and vulnerable protocol.");
			AnsiConsole.MarkupLine($"In fact, we ship a tool called [yellow]telnetpwn[/] in our repos that exploits Telnet to reveal user passwords.");
			AnsiConsole.MarkupLine($"[blue]Exercise[/]: Install the [yellow]telnetpwn[/] package using the [yellow]apt[/] package manager.");
			Console.WriteLine();
			AnsiConsole.MarkupLine($"[bold]Exploitation[/]");
			AnsiConsole.MarkupLine($"Hacking Telnet is as simple as running [yellow]telnetpwn [[hostname]][/]. It'll try and extract passwords for all users on a network.");
			AnsiConsole.MarkupLine($"[blue]Exercise[/]: Run [yellow]telnetpwn test.com[/].");
			Console.WriteLine();
			AnsiConsole.MarkupLine($"[bold]Post-Exploitation[/]");
			AnsiConsole.MarkupLine($"Once [yellow]telnetpwn[/] does its magic, you can perform post-exploitation, AKA the best part. In this case, you can [yellow]ssh[/] into the [yellow]root[/] user.");
			AnsiConsole.MarkupLine($"Better yet, the passwords you discover can be used in other, more secure environments, such as mail servers. Those are loads of fun.");
			AnsiConsole.MarkupLine($"A more clever move would be to gain SSH access for a low-level employee, and to start up a Telnet instance on that machine and discover other users.");
			AnsiConsole.MarkupLine($"[green]The sky is the limit here[/].");

			return true;
		}
	}
}
