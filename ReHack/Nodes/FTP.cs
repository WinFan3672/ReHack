using ReHack.Node;
using ReHack.BaseMethods;
using ReHack.Filesystem;

namespace ReHack.Node.FTP
{
	public class FTP : BaseNode
	{
		public bool Anonymous {get; } = false;
		public FTP(string Name, string UID, string Address, string? AdminPassword, VirtualDirectory? Root) : base(Name, UID, Address, new User[] {new User("root", AdminPassword, true), new User("ftp", null, false)})
		{
			if (Root.Name != "ftp")
			{
				throw new ArgumentException("Directory must be called 'ftp'");
			}
			this.Root.GetDirectory("var").AddDirectory(Root);
		}			
	}
}
