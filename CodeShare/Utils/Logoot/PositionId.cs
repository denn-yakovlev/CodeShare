using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{
    public struct PositionId : IEquatable<PositionId>, IComparable<PositionId>, IComparable
    {
        public Position Position { get; set; }

        public int Clock { get; set; }

        public PositionId(Position position, int clock)
        {
            Position = position;
            Clock = clock;
        }

        public static IEnumerable<PositionId> Generate(Interval<PositionId> interval, int amount, int site, int clock) =>
            Position
                .Generate((interval.Begin.Position, interval.End.Position), amount, site)
                .Select(pos => new PositionId(pos, clock));

        public int CompareTo(PositionId other)
        {
            var positionComparison = Position.CompareTo(other.Position);
            if (positionComparison != 0) return positionComparison;
            return Clock.CompareTo(other.Clock);
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is PositionId other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(PositionId)}");
        }

        public override bool Equals(object? obj)
        {
            return obj is PositionId id && Equals(id);
        }

        public bool Equals(PositionId other)
        {
            return Position.Equals(other.Position) &&
                   Clock == other.Clock;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Clock);
        }

        public override string ToString() => $"<{Position}, {Clock}>";

        public static PositionId Min { get; } = new PositionId(Position.Min, 0);
        public static PositionId Max { get; } = new PositionId(Position.Max, 0);

        public static bool operator ==(PositionId first, PositionId second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(PositionId first, PositionId second)
        {
            return !(first == second);
        }

        public static bool operator <(PositionId left, PositionId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(PositionId left, PositionId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(PositionId left, PositionId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(PositionId left, PositionId right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
