using ReHack.Node;
using ReHack.BaseMethods;
using Spectre.Console;
using ReHack.Exceptions;

namespace ReHack.Programs.Bank
{
	/// <summary>Send and receive money.</summary>
	public static class BankUtil
	{
		static string[] Reasons = new string[] {
			"Payment (Goods and Services)",
				"Payment (Loan)",
				"Donation",
				"Gift",
				"Bill Payment",
				"Investment",
				"Travel",
				"Medical Expenses",
				"Tax Payment",
				"Miscellaneous Expense",
		};

		static string GetReason()
		{
			return AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select transaction reason").AddChoices(Reasons));

		}

		/// <summary>Program function.</summary>
		public static bool Program(string[] Args, BaseNode Client, User RunningUser)
		{
			if (!RunningUser.Privileged)
			{
				throw new ErrorMessageException("Access denied");
			}
			if (Args.Length == 1 && Args.Contains("bal"))
			{
				AnsiConsole.MarkupLine($"Balance: [green]{Client.Balance}[/] credits");
			}
			else if (Args.Length == 2 && Args.Contains("log") && Args.Contains("sent"))
			{
				foreach(BankTransaction Transaction in Client.MoneySent)
				{
					Console.WriteLine(Transaction.ToString());
				}
			}
			else if (Args.Length == 2 && Args.Contains("log") && Args.Contains("recv"))
			{
				foreach (BankTransaction Transaction in Client.MoneyReceived)
				{
					Console.WriteLine(Transaction.ToString());
				}
			}
			else if (Args.Length == 3 && Args.Contains("send") && Array.IndexOf(Args, "send") == 0)
			{
				string Address = Args[1];
				int Amount = Convert.ToInt32(Args[2]);
				BankTransaction Transaction = new BankTransaction(Client.Address, Address, Amount, GetReason());
				if (!AnsiConsole.Confirm($"Are you sure you want to transfer {Amount} credits to {Address}?", false))
				{
					throw new ErrorMessageException("Transaction canceled by user");
				}
				if (BankUtils.PerformTransaction(Transaction))
				{
					AnsiConsole.MarkupLine("The transaction completed [green]successfully[/].");
				}
				else
				{
					throw new ErrorMessageException("Transaction failed");
				}
			}
			else if (Args.Length == 2 && Args.Contains("log") && Args.Contains("all"))
			{
				foreach (BankTransaction Transaction in Client.MoneyHandled)
				{
					Console.WriteLine(Transaction.ToString());
				}
			}
			else
			{
				AnsiConsole.MarkupLine("[bold blue]usage[/]: bankutil [[args]]");
				AnsiConsole.MarkupLine("Positional arguments:");
				AnsiConsole.MarkupLine("\tbal - View your balance");
				AnsiConsole.MarkupLine("\tsend [[ip addr]] [[amount]] - Send money");
				AnsiConsole.MarkupLine("\tlog [[sent|recv|all]] - See a list of transactions");
			}

			return true;
		}
	}
}
