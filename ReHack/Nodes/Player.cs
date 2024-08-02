using ReHack.BaseMethods;
using ReHack.Node;
using ReHack.Data;
using ReHack.Filesystem;
using Spectre.Console;
using ReHack.Networks;

namespace ReHack.Node.Player
{
    public class PlayerNode : BaseNode {
        public PlayerNode(string Username, string Password) : base("Localhost", "localhost", "127.0.0.1", new[] {new User(Username, Password, true)}, new AreaNetwork()) {
			// Add ports
			this.Ports.Add(GameData.GetPort("ssh"));
			this.Ports.Add(GameData.GetPort("rehack"));
			
			// Add debug
			if (DebugUtils.IsDebug())
			{
				this.AddProgram("debug");
			}
			
			this.AddProgram("welcome");
			this.AddProgram("tutorial");

			VirtualFile AptSources = Root.GetFile("/etc/apt/sources.list");
			AptSources.Content = AptSources.Content + "\npkg.rehack.org";
        }

		public override void Welcome()
		{
			if (!DebugUtils.IsDebug())
			{
				Console.Clear(); // Disabled in debug mode to let me see the warnings
			}
			AnsiConsole.MarkupLine("Welcome to [yellow]ReHackOS[/] [green]1.0[/]!");
			AnsiConsole.MarkupLine("For a command list, the [blue]help[/] command comes in handy.");
			AnsiConsole.MarkupLine("We use [blue]apt[/] as a package manager (program that installs other programs).");
			AnsiConsole.MarkupLine("To get per-program help, you can use [blue]man[/] and [blue]lsman[/].");
			Console.WriteLine();
			AnsiConsole.MarkupLine("For agent-specific help, the [blue]welcome[/] and [blue]tutorial[/] commands are your friend.");
			AnsiConsole.MarkupLine("We hope you enjoy working as a ReHack Agent.");


		}
    }
}
