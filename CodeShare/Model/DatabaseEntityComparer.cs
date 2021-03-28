using CodeShare.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model
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
