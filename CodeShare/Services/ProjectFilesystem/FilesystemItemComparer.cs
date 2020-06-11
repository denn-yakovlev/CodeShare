using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.ProjectFilesystem
{
    public class FilesystemItemComparer : IEqualityComparer<IFilesystemItem>
    {
        public bool Equals([AllowNull] IFilesystemItem x, [AllowNull] IFilesystemItem y) => 
            x != null && y != null && x.FullPath == y.FullPath;

        public int GetHashCode([DisallowNull] IFilesystemItem obj) => 
            HashCode.Combine(obj.FullPath);
    }
}
