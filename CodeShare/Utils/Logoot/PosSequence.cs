using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{ 
    readonly struct PosSequence : IEnumerable<uint>, IEquatable<PosSequence>, IComparable<PosSequence>
    {
        private readonly BigInteger _bigint;

        private readonly IEnumerable<uint> _sequence;

        private readonly byte[] _asBytes;

        public PosSequence(Position position)
        {
            _sequence = position.Select(id => id.Pos);
            _asBytes = _sequence
                .Reverse()
                .SelectMany(p => BitConverter.GetBytes(p))
                .ToArray();
            _bigint = new BigInteger(_asBytes);
        }

        //public static explicit operator PosSequence(Position position) =>
        //    new PosSequence(position);

        public override string ToString() => $"[{string.Join(',', _sequence)}]";

        

        public static IEnumerable<PosSequence> Generate(Interval<PosSequence> interval, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException();

            var bigintBegin = interval.Begin._bigint;
            var bigintEnd = interval.End._bigint;

            if (bigintBegin >= bigintEnd)
                throw new ArgumentException();

            var prefixLen = 0;
            var prefixesDistance = BigInteger.Zero;
            while (prefixesDistance < amount)
            {
                prefixLen++;
                prefixesDistance = interval.End.GetPrefixAsBigInt(prefixLen) 
                    - interval.Begin.GetPrefixAsBigInt(prefixLen);
            }

            var step = prefixesDistance / amount;
            var current = interval.Begin.GetPrefixAsBigInt(prefixLen);
            for (int i = 0; i < amount; i++)
            {
                var randomBigInt = current + GetRandomBigInt(1, step);
                yield return new PosSequence(randomBigInt);
                current += step;
            }
        }

        private PosSequence(BigInteger bigint)
        {
            _bigint = bigint;
            _asBytes = bigint.ToByteArray();

            var padLen = sizeof(uint) - _asBytes.Length % sizeof(uint);
            _sequence = _asBytes
                .Concat(Enumerable.Repeat<byte>(0, padLen))
                .Select((@byte, i) => (@byte, i))
                .GroupBy(pair => pair.i / sizeof(uint), pair => pair.@byte)
                .Reverse()
                .Select(bytesGroup => BitConverter.ToUInt32(bytesGroup.ToArray(), 0));       
        }

        private static BigInteger GetRandomBigInt(BigInteger first, BigInteger second)
        {
            if (first > second)
                throw new ArgumentException();

            var random = new Random();
            var diff = second - first;
            var diffBytesCnt = diff.GetByteCount();
            var buf = new byte[diffBytesCnt];
            var span = new Span<byte>(buf);
            BigInteger result;
            do
            {
                var fillLen = random.Next(diffBytesCnt + 1);
                var slice = span.Slice(0, fillLen);
                span.Clear();
                random.NextBytes(slice);
                MakePositiveValue(slice);
                result = new BigInteger(slice);
            } while (result > diff);
            return first + result;
        }

        private static void MakePositiveValue(Span<byte> slice)
        {
            if (slice.Length == 0)
                return;
            slice[^1] &= 0b01111111;
        }

        // pads with 0 if prefixLen < seqLen
        private BigInteger GetPrefixAsBigInt(int prefixLen)
        {
            var seqLen = _sequence.Count();
            var padLen = prefixLen <= seqLen ? 0 : prefixLen - seqLen;
            return new BigInteger(_sequence
                .Take(prefixLen)
                .Concat(Enumerable.Repeat<uint>(0, padLen))
                .Reverse()
                .SelectMany(i => BitConverter.GetBytes(i))
                .ToArray()
                );
        }

        public IEnumerator<uint> GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_sequence).GetEnumerator();
        }

        public bool Equals([AllowNull] PosSequence other) => 
            _bigint.Equals(other._bigint);

        public int CompareTo([AllowNull] PosSequence other) => 
            _bigint.CompareTo(other._bigint);

        public override int GetHashCode() => 
            HashCode.Combine(_bigint);

        public static bool operator ==(PosSequence left, PosSequence right) => 
            left.Equals(right);

        public static bool operator !=(PosSequence left, PosSequence right) => 
            !(left == right);
    }
}
