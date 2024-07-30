namespace ReHack;

using ReHack.Welcome;
using ReHack.Menus;
using ReHack.BaseMethods;

class Program
{
    static void Main(string[] args)
    {
		Menu MainMenu = new Menu("ReHack");
		MainMenu.AddOption("New Game", WelcomeSequence.Init);
		MainMenu.AddExitOption();
		if (DebugUtils.IsDebug())
		{
			WelcomeSequence.Init();
		}
		else
		{
			MainMenu.Run();
		}
    }
}
