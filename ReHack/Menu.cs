using ReHack.BaseMethods;
using System.Collections.Generic;

namespace ReHack.Menus
{
	public class Menu
	{
		/// <summary>
		/// Class for displaying menus. Calls functions upon user selection. Functions must return bools, where returning true tells the menu to continue the menu loop.
		/// </summary>
		public string Title {get; set; }
		public List<string> Options {get; } = new List<string>();
		public List<Func<bool>> Actions {get; } = new List<Func<bool>>();

		public Menu(string Title)
		{
			this.Title = Title;
		}

		public void AddOption(string Option, Func<bool> Action)
		{
			if (Option == null) throw new ArgumentNullException(nameof(Option));
			if (Action == null) throw new ArgumentNullException(nameof(Action));
			this.Options.Add(Option);
			this.Actions.Add(Action);
		}

		public void AddExitOption(string Message="Exit")
		{
			this.AddOption(Message, () => { return false; } );
		}

		public virtual void Run()
		{
			bool Continue = true;
			while (Continue)
			{
				Console.Clear();
				PrintUtils.Divider();
				Console.WriteLine(this.Title);
				PrintUtils.Divider();
				for (int i = 0; i < this.Options.Count; i++)
				{
					Console.WriteLine($"{i+1}: {Options[i]}");
				}
				PrintUtils.Divider();
				Console.Write("Select an option >");
				if (int.TryParse(Console.ReadLine(), out int Selection) && Selection > 0 && Selection <= this.Options.Count)
				{
					Continue = this.Actions[Selection - 1]();
				}
			}
		}
	}

	public class ArrowKeyMenu : Menu
	{
		public ArrowKeyMenu(string Title) : base(Title) {}
		public override void Run()
		{
			bool continueMenu = true;
			int selectedOption = 0;

			while (continueMenu)
			{
				Console.Clear();
				PrintUtils.Divider();
				Console.WriteLine(Title);
				PrintUtils.Divider();

				for (int i = 0; i < Options.Count; i++)
				{
					if (i == selectedOption)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"> {Options[i]}");
						Console.ResetColor();
					}
					else
					{
						Console.WriteLine($"  {Options[i]}");
					}
				}

				PrintUtils.Divider();

				var key = Console.ReadKey(intercept: true).Key;

				switch (key)
				{
					case ConsoleKey.UpArrow:
						selectedOption = (selectedOption > 0) ? selectedOption - 1 : Options.Count - 1;
						break;
					case ConsoleKey.DownArrow:
						selectedOption = (selectedOption < Options.Count - 1) ? selectedOption + 1 : 0;
						break;
					case ConsoleKey.Enter:
						continueMenu = Actions[selectedOption]();
						break;
				}
			}
		}
	}

}
