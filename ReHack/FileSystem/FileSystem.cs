using ReHack.Node;
using Spectre.Console;
using ReHack.BaseMethods;

namespace ReHack.Filesystem
{

	public interface IVirtualFile
	{
		string Name {get; set; }
		void View(BaseNode Client);
	}

	public class VirtualFile : IVirtualFile
	{
		public string Name { get; set; }
		public string Content { get; set; }

		public VirtualFile(string name, string content = "")
		{
			this.Name = name;
			this.Content = content;
		}

		public void View(BaseNode Client)
		{
			bool Running = true;
			while (Running)
			{
				Console.Clear();
				string Option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(Name).AddChoices(new[] {"View Contents", "Back" }));
				switch (Option)
				{
					case "View Contents":
						Console.Clear();
						PrintUtils.Divider();
						Console.WriteLine(Content);
						PrintUtils.Divider();
						PrintUtils.WaitForContinue();
						break;
					case "Back":
						Running = false;
						break;
				}
			}
		}

		public VirtualFile Clone()
		{
			return new VirtualFile(new String(Name), new String(Content));
		}
	}

	public class VirtualDirectory : IVirtualFile
	{
		public string Name { get; set; }
		public List<VirtualFile> Files { get; set; }
		public List<VirtualDirectory> SubDirectories { get; set; }

		public VirtualDirectory(string name, VirtualFile[] Files, VirtualDirectory[] Dirs)
		{
			this.Name = name;
			this.Files = new List<VirtualFile>();
			this.SubDirectories = new List<VirtualDirectory>();
			foreach (VirtualFile File in Files)
			{
				this.Files.Add(File);
			}
			foreach (VirtualDirectory Dir in Dirs)
			{
				this.SubDirectories.Add(Dir);
			}
		}

		public void AddFile(VirtualFile file)
		{
			this.Files.Add(file);
		}

		public void AddDirectory(VirtualDirectory directory)
		{
			this.SubDirectories.Add(directory);
		}

		public VirtualFile GetFile(string name)
		{
			return this.Files.FirstOrDefault(f => f.Name == name) ?? throw new FileNotFoundException();
		}

		public VirtualDirectory GetDirectory(string name)
		{
			return this.SubDirectories.FirstOrDefault(d => d.Name == name) ?? throw new FileNotFoundException();
		}

		public void View(BaseNode Client)
		{
			bool Running = true;
			while (Running)
			{
				Console.Clear();
				Dictionary<string, IVirtualFile> SubFiles = EnumFiles();
				string Selection = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(this.Name).AddChoices(SubFiles.Keys).AddChoices(new[] {"Back"}));
				if (Selection == "Back")
				{
					Running = false;
				}
				else
				{
					SubFiles[Selection].View(Client);
				}

			}
		}		

		public Dictionary<string, IVirtualFile> EnumFiles()
		{
			Dictionary<string, IVirtualFile> Files = new Dictionary<string, IVirtualFile>();
			foreach(IVirtualFile File in this.Files)
			{
				Files[File.Name] = File;
			}
			foreach(IVirtualFile File in this.SubDirectories)
			{
				Files[File.Name] = File;
			}
			return Files;
		}

		public VirtualDirectory Clone()
		{
			List<VirtualFile> NewFiles = new List<VirtualFile>();
			foreach(VirtualFile File in this.Files)
			{
				NewFiles.Add(File.Clone());
			}
			List<VirtualDirectory> NewDirs = new List<VirtualDirectory>();
			foreach(VirtualDirectory Dir in this.SubDirectories)
			{
				NewDirs.Add(Dir.Clone());
			}
			return new VirtualDirectory(new String(Name), NewFiles.ToArray(), NewDirs.ToArray());
		}
	}

	public class FileSystem : IVirtualFile
	{
		public string Name {get; set; } = "Root"; // Compliance with IVirtualFile
		public VirtualDirectory Root { get; private set; }

		public FileSystem(VirtualFile[] Files, VirtualDirectory[] Directories)
		{
			this.Root = new VirtualDirectory("Root", Files, Directories);
		}

		public VirtualDirectory GetDirectory(string path)
		{
			string[] parts = path.Split('/');
			VirtualDirectory? current = this.Root;

			foreach (string part in parts)
			{
				if (part == "") continue;

				current = current.GetDirectory(part);
			}

			return current;
		}

		public void View(BaseNode Client)
		{
			Root.View(Client);
		}

		public void AddFile(string path, VirtualFile file)
		{
			string[] parts = path.Split('/');
			VirtualDirectory current = this.Root;

			for (int i = 0; i < parts.Length - 1; i++)
			{
				string part = parts[i];
				if (part == "") continue;

				VirtualDirectory nextDir = current.GetDirectory(part);
				if (nextDir == null)
				{
					nextDir = new VirtualDirectory(part, new VirtualFile[]{}, new VirtualDirectory[]{});
					current.AddDirectory(nextDir);
				}
				current = nextDir;
			}

			current.AddFile(file);
		}

		public VirtualFile? GetFile(string path)
		{
			string[] parts = path.Split('/');
			VirtualDirectory? current = this.Root;

			for (int i = 0; i < parts.Length - 1; i++)
			{
				string part = parts[i];
				if (part == "") continue;

				current = current.GetDirectory(part);
				if (current == null) return null;
			}

			return current.GetFile(parts.Last());
		}

		public void DeleteFile(string Path)
		{
			VirtualFile? File = GetFile(Path);
			if (File == null)
			{
				throw new ArgumentException("Invalid file");
			}
			List<string> Parts = new List<string>(Path.Split("/"));
			Parts.Remove(File.Name);
			string DirPath = string.Join("/", Parts);
			VirtualDirectory? Directory = GetDirectory(DirPath);
			Directory.Files.Remove(File);
		}

		public FileSystem Clone()
		{
			VirtualDirectory NewRoot = Root.Clone();
			return new FileSystem(NewRoot.Files.ToArray(), NewRoot.SubDirectories.ToArray());
		}
	}

}
