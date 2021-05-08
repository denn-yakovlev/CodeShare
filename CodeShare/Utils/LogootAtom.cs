using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils
{
    public struct LogootAtom : IComparable<LogootAtom>, IEquatable<LogootAtom>
    {
        public LogootId Id { get; set; }
        public char Char { get; set; }

        public LogootAtom(LogootId id, char @char)
        {
            Id = id;
            Char = @char;
        }

        public static LogootAtom First { get; } = new LogootAtom(LogootId.Min, (char)0);
        public static LogootAtom Last { get; } = new LogootAtom(LogootId.Max, (char)0);

        public int CompareTo([AllowNull] LogootAtom other) => 
            Id.CompareTo(other.Id);

        public bool Equals([AllowNull] LogootAtom other) => 
            Id.Equals(other.Id);

        public override string ToString()
        {
            return $"{{{Id}, {Char}}}";
        }
    }
}
