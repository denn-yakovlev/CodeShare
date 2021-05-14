using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{
    public struct Identifier : IEquatable<Identifier>, IComparable<Identifier>
    {
        public uint Pos { get; }

        public int Site { get; }

        //private static Random _random = new Random();

        public Identifier(uint pos, int site)
        {
            Pos = pos;
            Site = site;
        }

        public int CompareTo(Identifier other)
        {
            return Pos == other.Pos 
                ? Site.CompareTo(other.Site) 
                : Pos.CompareTo(other.Pos);
        }

        public bool Equals(Identifier other)
        {
            return Pos == other.Pos && Site == other.Site;
        }

        public override bool Equals(object? obj)
        {
            return obj is Identifier other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pos, Site);
        }

        public override string ToString() => $"({Pos}, {Site})";

        public static bool operator ==(Identifier left, Identifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !left.Equals(right);
        }

        //public static Identifier GenerateOneRandomBetween(Identifier begin, Identifier end, int site)
        //{
        //    if (begin.CompareTo(end) >= 0)
        //        throw new ArgumentException();
        //    return new Identifier(_random.Next(begin.Pos + 1, end.Pos), site);
        //}

        //public static IEnumerable<Identifier> GenerateRandomIdssBetween(
        //    Identifier begin, Identifier end, int amount, int site
        //    )
        //{
        //    if (begin.CompareTo(end) >= 0)
        //        throw new ArgumentException();
        //    var betweenCount = end.Pos - begin.Pos - 1;
        //    var chunkSize = betweenCount / amount;
        //    var chunkBegin = begin.Pos + 1;
        //    while (chunkBegin < end.Pos)
        //    {
        //        yield return new Identifier(_random.Next(chunkBegin, chunkBegin + chunkSize), site);
        //        chunkBegin += chunkSize;
        //    }

        //}
    }
}
