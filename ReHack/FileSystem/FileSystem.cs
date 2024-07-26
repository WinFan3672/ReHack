using System.Collections.Generic;

namespace ReHack.Filesystem
{
    public class Filesystem {
        private Directory Root {get; }

        public Filesystem(string Name = "root") {
            Root = new Directory(Name);
        }
    }


    public class File
    {
        public string Name {get; set; }
        public string Content {get; set; }

        public File(string Name, string Content) {
            this.Name = Name;
            this.Content = Content;
        }
    }

    public class Directory
    {
        public string Name {get; set; }
        public Dictionary<string, File> Files { get; }
        public Dictionary<string, Directory> Directories { get; }

        public Directory(string Name)
        {
            this.Name = Name;
            this.Files = new Dictionary<string, File>();
            this.Directories = new Dictionary<string, Directory>();
        }
    }
}
