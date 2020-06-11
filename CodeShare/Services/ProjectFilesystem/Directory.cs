using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace CodeShare.Services.ProjectFilesystem
{
    public class Directory : IFilesystemItem
    {
        public static Directory Root { get; } = new Directory("", null, true);

        public ICollection<IFilesystemItem> Items { get; } = new List<IFilesystemItem>();

        private Directory() { }

        private Directory(string name, Directory parent, bool isRoot = false) 
        { 
            Name = name;
            if (isRoot)
            {
                FullPath = "/";
            }
            else
            {
                FullPath = parent.FullPath + Name + "/";
            }
            
        }

        public string Name { get; }

        public string FullPath { get; }

        public Directory Dir(string name)
        {
            var item = (Directory)Items.SingleOrDefault(item => item.Name == name);
            if (item == null)
            {
                var newDir = new Directory(name, this);
                Items.Add(newDir);
                return newDir;
            }
            else
            {
                return item;
            }
        }

        public File File(string name)
        {
            var item = (File)Items.SingleOrDefault(item => item.Name == name);
            if (item == null)
            {
                var newFile = new File(name, this);
                Items.Add(newFile);
                return newFile;
            }
            else
            {
                return item;
            }
        }

        public override string ToString() => FullPath;
    }
}
