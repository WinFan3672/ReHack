using ReHack.Data;
using ReHack.BaseMethods;
using ReHack.Node;

namespace ReHack.Programs.TelnetClient
{
    public static class TelnetClient
    {
        public static bool Program(BaseNode Client)
        {
            if (NodeUtils.CheckPort(Client, "telnet"))
            {
                return true;
            }
            return false;
        }
    }
}
