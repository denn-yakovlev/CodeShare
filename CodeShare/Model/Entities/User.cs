using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class User : IEquatable<User>
    {
        public string Name { get; set; } = string.Empty;

        public bool Equals([AllowNull] User other) =>
            other != null && other.Name == Name;
    }
}
