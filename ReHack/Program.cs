namespace ReHack;

using ReHack.Welcome;
using ReHack.Menus;
using ReHack.BaseMethods;
using Spectre.Console;

class Program
{
	static void Main(string[] args)
	{
		/*ArrowKeyMenu MainMenu = new ArrowKeyMenu("ReHack");*/
		/*MainMenu.AddOption("New Game", WelcomeSequence.Init);*/
		/*MainMenu.AddExitOption();*/
		/*if (DebugUtils.IsDebug())*/
		/*{*/
		/*	WelcomeSequence.Init();*/
		/*}*/
		/*else*/
		/*{*/
		/*	MainMenu.Run();*/
		/*}*/
		if (DebugUtils.IsDebug())
		{
			WelcomeSequence.Init();
		}
		else
		{	
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
