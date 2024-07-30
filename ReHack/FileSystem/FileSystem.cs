using System;
using System.Collections.Generic;
using System.Linq;

namespace ReHack.Filesystem
{

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

		public VirtualDirectory(string name)
		{
			this.Name = name;
			this.Files = new List<VirtualFile>();
			this.SubDirectories = new List<VirtualDirectory>();
		}

		public void AddFile(VirtualFile file)
		{
			this.Files.Add(file);
		}

		public void AddDirectory(VirtualDirectory directory)
		{
			this.SubDirectories.Add(directory);
		}

		public VirtualFile FindFile(string name)
		{
			return this.Files.FirstOrDefault(f => f.Name == name);
		}

		public VirtualDirectory FindDirectory(string name)
		{
			return this.SubDirectories.FirstOrDefault(d => d.Name == name);
		}
	}

	public class FileSystem
	{
		public VirtualDirectory Root { get; private set; }

		public FileSystem()
		{
			this.Root = new VirtualDirectory("Root");
		}

		public VirtualDirectory GetDirectory(string path)
		{
			string[] parts = path.Split('/');
			VirtualDirectory? current = this.Root;

			foreach (string part in parts)
			{
				if (part == "") continue;

				current = current.FindDirectory(part);
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

				VirtualDirectory nextDir = current.FindDirectory(part);
				if (nextDir == null)
				{
					nextDir = new VirtualDirectory(part);
					current.AddDirectory(nextDir);
				}
				current = nextDir;
			}

			current.AddFile(file);
		}

		public VirtualFile? FindFile(string path)
		{
			string[] parts = path.Split('/');
			VirtualDirectory? current = this.Root;

			for (int i = 0; i < parts.Length - 1; i++)
			{
				string part = parts[i];
				if (part == "") continue;

				current = current.FindDirectory(part);
				if (current == null) return null;
			}

			return current.FindFile(parts.Last());
		}
	}

}
