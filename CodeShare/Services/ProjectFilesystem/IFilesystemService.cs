using CodeShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.ProjectFilesystem
{
    public interface IFilesystemService
    {
        //IEnumerable<Lazy<Directory>> ProjectDirectories { get; }

        Directory Root { get; }

        void Mount(object sender, ConnToProjectEventArgs e);

        void Unmount(object sender, ConnToProjectEventArgs e);
    }
}
