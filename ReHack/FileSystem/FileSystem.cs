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
			Name = name;
			Content = content;
		}
	}

	public class Directory
	{
		public string Name { get; set; }
		public List<VirtualFile> Files { get; set; }
		public List<Directory> SubDirectories { get; set; }

		public Directory(string name)
		{
			Name = name;
			Files = new List<VirtualFile>();
			SubDirectories = new List<Directory>();
		}

		public void AddFile(VirtualFile file)
		{
			Files.Add(file);
		}

		public void AddDirectory(Directory directory)
		{
			SubDirectories.Add(directory);
		}

		public VirtualFile FindFile(string name)
		{
			return Files.FirstOrDefault(f => f.Name == name);
		}

		public Directory FindDirectory(string name)
		{
			return SubDirectories.FirstOrDefault(d => d.Name == name);
		}
	}

	public class FileSystem
	{
		public Directory Root { get; private set; }

		public FileSystem()
		{
			Root = new Directory("Root");
		}

		public Directory GetDirectory(string path)
		{
			string[] parts = path.Split('/');
			Directory current = Root;

			foreach (string part in parts)
			{
				if (part == "") continue;

				current = current.FindDirectory(part);
				if (current == null) return null;
			}

			return current;
		}

		public void AddFile(string path, VirtualFile file)
		{
			string[] parts = path.Split('/');
			Directory current = Root;

			for (int i = 0; i < parts.Length - 1; i++)
			{
				string part = parts[i];
				if (part == "") continue;

				Directory nextDir = current.FindDirectory(part);
				if (nextDir == null)
				{
					nextDir = new Directory(part);
					current.AddDirectory(nextDir);
				}
				current = nextDir;
			}

			current.AddFile(file);
		}

		public VirtualFile FindFile(string path)
		{
			string[] parts = path.Split('/');
			Directory current = Root;

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
