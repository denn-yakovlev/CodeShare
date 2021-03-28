using CodeShare.Services.CollabManager;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CodeShare.Utils
{
    public class CollaboratorInfoComparer : IEqualityComparer<CollaboratorInfo>
    {
        public bool Equals([AllowNull] CollaboratorInfo x, [AllowNull] CollaboratorInfo y)
        {
            return x != null && y != null &&
                x.UserName == y.UserName &&
                x.PermissionsInProject.SequenceEqual(y.PermissionsInProject) &&
                x.ConnectionIds.SequenceEqual(y.ConnectionIds) &&
                x.IsAnonymous == y.IsAnonymous;
        }

        public int GetHashCode([DisallowNull] CollaboratorInfo obj)
        {
            return HashCode.Combine(obj.UserName, obj.PermissionsInProject, obj.ConnectionIds, obj.IsAnonymous);
        }
    }
}
