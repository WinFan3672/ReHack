using ReHack.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReHack.Filesystem
{

	public interface IVirtualFile
	{
		void View(BaseNode Client);
	}

	public class VirtualFile
	{
		public string Name { get; set; }
		public string Content { get; set; }

		public VirtualFile(string name, string content = "")
		{
			this.Name = name;
			this.Content = content;
		}
	}

	public class VirtualDirectory
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
			return this.Files.FirstOrDefault(f => f.Name == name);
		}

		public VirtualDirectory GetDirectory(string name)
		{
			return this.SubDirectories.FirstOrDefault(d => d.Name == name);
		}
	}

	public class FileSystem
	{
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
	}

}
