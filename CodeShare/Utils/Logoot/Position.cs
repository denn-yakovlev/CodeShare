using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CodeShare.Utils.Logoot
{
    public class Position : IEnumerable<Identifier>, ICollection<Identifier>, IEquatable<Position>, IComparable<Position>, IComparable
    {
        public IList<Identifier> Identifiers { get; set; }

        public int Length => Identifiers.Count;
        
        public static Position Min { get; } = new Position(new Identifier(int.MinValue, -1));
        public static Position Max { get; } = new Position(new Identifier(int.MaxValue, -1));

        public Position()
        {
            Identifiers = new List<Identifier>();
        }
        
        public Position(params Identifier[] identifiers)
        {
            Identifiers = new List<Identifier>(identifiers);
        }

        public Position(IEnumerable<Identifier> identifiers)
        {
            Identifiers = identifiers.ToList();
        }

        public Identifier this[int index] => Identifiers[index];

        public IEnumerator<Identifier> GetEnumerator() =>
            Identifiers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            Identifiers.GetEnumerator(); 
        //return ((IEnumerable)Identifiers).GetEnumerator();
        
        public override bool Equals(object? obj) => 
            obj is Position position && Equals(position);

        public bool Equals(Position other) =>
            new PosSequence(this).Equals(new PosSequence(other));

        public override int GetHashCode() =>
            HashCode.Combine(new PosSequence(this).GetHashCode());

        public override string ToString() => $"[{string.Join(',', Identifiers)}]";

        public static IEnumerable<Position> Generate(Interval<Position> interval, int amount, int site)
        {
            if (interval.Begin >= interval.End)
                throw new ArgumentException($"{interval.Begin} must be less than {interval.End}");
                
            var posSeqInterval = (
                    new PosSequence(interval.Begin),
                    new PosSequence(interval.End)
                );
            var posSequences = PosSequence.Generate(posSeqInterval, amount);
            return posSequences.Select(pSeq => new Position(IdsFromPosSequence(pSeq, interval, site)));
        }

        public int CompareTo(Position other) => 
            new PosSequence(this).CompareTo(new PosSequence(other));

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is Position other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Position)}");
        }

        public static bool operator <(Position left, Position right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Position left, Position right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Position left, Position right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Position left, Position right)
        {
            return left.CompareTo(right) >= 0;
        }

        private static IEnumerable<Identifier> IdsFromPosSequence(PosSequence pSeq, Interval<Position> interval, int site)
        {
            var last = pSeq.Last();
            int i = 0;
            foreach (var pos in pSeq)
            {
                int currentSite;
                if (i < interval.Begin.Length &&  pos == interval.Begin[i].Pos)
                    currentSite = interval.Begin[i].Site;
                else if (i < interval.Begin.Length && pos == interval.End[i].Pos)
                    currentSite = interval.End[i].Site;
                else
                    currentSite = site;
                if (pos == last)
                    currentSite = site;
                yield return new Identifier(pos, currentSite);
                i++;
            }
        }


        #region only for json deserialization
        void ICollection<Identifier>.Add(Identifier item)
        {
            Identifiers.Add(item);
        }

        void ICollection<Identifier>.Clear()
        {
            Identifiers.Clear();
        }

        bool ICollection<Identifier>.Contains(Identifier item)
        {
            return Identifiers.Contains(item);
        }

        void ICollection<Identifier>.CopyTo(Identifier[] array, int arrayIndex)
        {
            Identifiers.CopyTo(array, arrayIndex);
        }

        bool ICollection<Identifier>.Remove(Identifier item)
        {
            return Identifiers.Remove(item);
        }

        int ICollection<Identifier>.Count => Identifiers.Count;

        bool ICollection<Identifier>.IsReadOnly => false;
        #endregion
    }
}
