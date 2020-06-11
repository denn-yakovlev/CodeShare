using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.ProjectFilesystem
{
    public class File : IFilesystemItem
    {
        public File(string name, Directory dir)
        {
            Name = name;
            FullPath = dir.FullPath + Name;
        }

        public string Name { get; }

        public string FullPath { get; }

        public Models.File ToModel() => 
            new Models.File { Name = Name, Path = FullPath };

        public override string ToString() => FullPath;
    }
}
