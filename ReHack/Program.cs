namespace ReHack;
using ReHack.Welcome;
using ReHack.BaseMethods;
using Spectre.Console;

class Program
{
	static void Main(string[] args)
	{
		if (DebugUtils.IsDebug())
		{
			WelcomeSequence.Init();
		}
		else
		{
			Console.Clear();
			string MainMenu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("ReHack").AddChoices("New Game", "Exit"));

			switch (MainMenu)
			{
				case "New Game":
					WelcomeSequence.Init();
					break;
				case "Exit":
					break;
			}
		}

	}
}
