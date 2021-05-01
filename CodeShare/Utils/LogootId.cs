using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils
{
    public struct LogootId : IComparable<LogootId>, IEquatable<LogootId>
    {
        public decimal Position { get; }

        public int SiteId { get; }

        public int SiteClockValue { get; }

        public LogootId(decimal position, int siteId, int siteClockValue)
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

        public static LogootId Min { get; } = new LogootId(0.0m, -1, -1);
        public static LogootId Max { get; } = new LogootId(1.0m, -1, -1);
    }
}
