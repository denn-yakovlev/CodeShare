using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{
    public struct Position : IEnumerable<Identifier>, IEquatable<Position>, IComparable<Position>
    {
        private IList<Identifier> _identifiers;

        //private PosSequence _posSequence;

        public Position(params Identifier[] identifiers)
        {
            _identifiers = identifiers;
        }

        public Position(IEnumerable<Identifier> identifiers)
        {
            _identifiers = identifiers.ToList();
        }

        public int Length => _identifiers.Count;

        public Identifier this[int index] => _identifiers[index];

        public IEnumerator<Identifier> GetEnumerator() => 
            _identifiers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_identifiers).GetEnumerator();
        }

        public override bool Equals(object? obj) => 
            obj is Position position && Equals(position);

        public bool Equals(Position other) =>
            new PosSequence(this).Equals(new PosSequence(other));

        public override int GetHashCode() =>
            HashCode.Combine(new PosSequence(this).GetHashCode());

        public override string ToString() => string.Join(',', _identifiers);

        public static IEnumerable<Position> Generate(Interval<Position> interval, int amount, int site)
        {
            var posSeqInterval = new Interval<PosSequence>(
                    new PosSequence(interval.Begin),
                    new PosSequence(interval.End)
                );
            var posSequences = PosSequence.Generate(posSeqInterval, amount);
            return posSequences.Select(pSeq => new Position(IdsFromPosSequence(pSeq, interval, site)));
        }

        private static IEnumerable<Identifier> IdsFromPosSequence(PosSequence pSeq, Interval<Position> interval, int site)
        {
            var last = pSeq.Last();
            int i = 0;
            int currentSite;
            foreach (var pos in pSeq)
            {
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

        public int CompareTo([AllowNull] Position other) =>
            new PosSequence(this).CompareTo(new PosSequence(other));
    }
}
