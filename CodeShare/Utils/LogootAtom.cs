using System;
using System.Diagnostics.CodeAnalysis;
using CodeShare.Utils.Logoot;

namespace CodeShare.Utils
{
    public struct LogootAtom : IComparable<LogootAtom>, IEquatable<LogootAtom>
    {
        public PositionId Id { get; set; }
        public char Char { get; set; }

        public LogootAtom(PositionId id, char @char)
        {
            Id = id;
            Char = @char;
        }

        public static LogootAtom First { get; } = new LogootAtom(PositionId.Min, (char)0);
        public static LogootAtom Last { get; } = new LogootAtom(PositionId.Max, (char)0);

        public int CompareTo([AllowNull] LogootAtom other) => 
            Id.CompareTo(other.Id);

        public bool Equals([AllowNull] LogootAtom other) => 
            Id.Equals(other.Id);

        public override string ToString() => 
            $"{{{Id}, {Char}}}";
    }
}
