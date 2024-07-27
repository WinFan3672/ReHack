using ReHack.Data;
using ReHack.BaseMethods;
using ReHack.Node;

namespace ReHack.Programs.SSHClient
{
    public static class SSHClient
    {
        private static void ServiceRunner(BaseNode Client, bool ConfirmExit=false)
        {
            string Input;
            while (true)
            {
                Console.Write($"{Client.Name}@{Client.Address} $");
                Input = Console.ReadLine();
                if (Input == "exit" || Input == "quit")
                {
                    return;
                }
                else {
                    Console.WriteLine(Input);
                }
            }
        }
        public static bool Program(BaseNode Client, bool ConfirmExit=false)
        {
            if (NodeUtils.CheckPort(Client, "ssh"))
            {
                ServiceRunner(Client, ConfirmExit);
                return true;
            }
            return false;
        }
    }
}
