using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeShare.Utils
{
    public class DatabaseEntityComparer : IEqualityComparer<DatabaseEntity>
    {
        public bool Equals([AllowNull] DatabaseEntity x, [AllowNull] DatabaseEntity y)
        {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] DatabaseEntity obj)
        {
            return HashCode.Combine(obj.Id);
        }
    }
}
