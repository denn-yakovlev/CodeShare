using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils
{
    public struct LogootId : IComparable<LogootId>, IEquatable<LogootId>
    {
        public double Position { get; set; }

        public int SiteId { get; set; }

        public int SiteClockValue { get; set; }

        public LogootId(double position, int siteId, int siteClockValue)
        {
            Position = position;
            SiteId = siteId;
            SiteClockValue = siteClockValue;
        }

        public int CompareTo([AllowNull] LogootId other)
        {
            if (Position == other.Position)
            {
                if (SiteId == other.SiteId)
                {
                    if (SiteClockValue == other.SiteClockValue)
                        return 0;
                    return SiteClockValue.CompareTo(other.SiteClockValue);
                }
                return SiteId.CompareTo(other.SiteId);
            }
            return Position.CompareTo(other.Position);
        }

        public override bool Equals(object? obj)
        {
            return obj is LogootId id && Equals(id);
        }

        public bool Equals(LogootId other)
        {
            return Position == other.Position &&
                   SiteId == other.SiteId &&
                   SiteClockValue == other.SiteClockValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, SiteId, SiteClockValue);
        }

        public override string? ToString()
        {
            return $"[{Position}, {SiteId}, {SiteClockValue}]";
        }

        public static bool operator ==(LogootId left, LogootId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogootId left, LogootId right)
        {
            return !(left == right);
        }

        public static bool operator <(LogootId left, LogootId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(LogootId left, LogootId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(LogootId left, LogootId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(LogootId left, LogootId right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static LogootId Min { get; } = new LogootId(0, -1, -1);
        public static LogootId Max { get; } = new LogootId(double.MaxValue, -1, -1);

        private static Random _random = new Random();

        public static IEnumerable<LogootId> GenerateBetween(
            LogootId firstId, LogootId secondId, int amount, int site, int clock
            )
        
        {
            if (firstId >= secondId)
                throw new ArgumentException($"{nameof(firstId)} must be lesser than {nameof(secondId)}");
            if (amount < 1)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Should be at least 1");
            if (amount == 1)
            {
                yield return new LogootId(
                    RandomDoubleBetween(firstId.Position, secondId.Position),
                    site,
                    clock
                );
            }
            else
            {
                var step = (secondId.Position - firstId.Position) / amount;
                var firstPos = firstId.Position;
                var secondPos = secondId.Position;
                while (firstPos < secondPos)
                {
                    yield return new LogootId(
                        RandomDoubleBetween(firstPos, firstPos + step),
                        site,
                        clock
                    );
                    firstPos += step;
                }
            }
        }

        private static double RandomDoubleBetween(double lo, double hi)
        {
            lo += double.Epsilon;
            return lo + _random.NextDouble() * (hi - lo);
        }
    }
}
