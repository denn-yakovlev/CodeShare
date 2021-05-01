using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils
{
    public struct LogootAtom : IComparable<LogootAtom>, IEquatable<LogootAtom>
    {
        public LogootId Id { get; }
        public char? Char { get; }

        public LogootAtom(LogootId id, char? @char)
        {
            Id = id;
            Char = @char;
        }

        public static LogootAtom First { get; } = new LogootAtom(LogootId.Min, null);
        public static LogootAtom Last { get; } = new LogootAtom(LogootId.Max, null);

        public int CompareTo([AllowNull] LogootAtom other) => 
            Id.CompareTo(other.Id);

        public bool Equals([AllowNull] LogootAtom other) => 
            Id.Equals(other.Id);
    }
}
