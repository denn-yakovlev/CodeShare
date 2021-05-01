using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class Solution : IEquatable<Solution>
    {
        public ProgrammingLanguage Language { get; set; } = ProgrammingLanguage.C;

        public string SourceCode { get; set; } = string.Empty;

        public bool Equals([AllowNull] Solution other) =>
            other != null && 
            ((IEquatable<ProgrammingLanguage>)other.Language).Equals(Language);
    }
}
