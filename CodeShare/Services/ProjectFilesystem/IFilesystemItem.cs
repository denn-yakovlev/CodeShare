using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeShare.Services.ProjectFilesystem
{
    public interface IFilesystemItem //: IEquatable<IFilesystemItem>
    {
        string Name { get; }

        string FullPath { get; }

        
    }
}
