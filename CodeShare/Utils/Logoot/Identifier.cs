using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{
    public struct Identifier : IEquatable<Identifier>, IComparable<Identifier>
    {
        public int Pos { get; set; }

        public int Site { get; set; }

        public Identifier(int pos, int site)
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
    }
}
