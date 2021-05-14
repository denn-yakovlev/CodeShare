using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{
    public struct PositionId : IEquatable<PositionId>
    {
        public Position Position { get; }

        public int Clock { get; }

        public PositionId(Position position, int clock)
        {
            Position = position;
            Clock = clock;
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

        public static PositionId Min { get; } = new PositionId(
            new Position(new Identifier(uint.MinValue, 0)), 0
            );

        public static PositionId Max { get; } = new PositionId(
            new Position(new Identifier(uint.MaxValue, 0)), 0
            );

        public static bool operator ==(PositionId first, PositionId second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(PositionId first, PositionId second)
        {
            return !(first == second);
        }
    }
}
