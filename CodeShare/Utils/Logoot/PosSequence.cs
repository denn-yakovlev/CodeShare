using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CodeShare.Utils.Logoot
{ 
    readonly struct PosSequence : IEnumerable<int>, IEquatable<PosSequence>, IComparable<PosSequence>
    {
        private readonly BigInteger _bigint;

        private readonly IEnumerable<int> _sequence;

        public PosSequence(Position position)
       {
            _sequence = position.Select(id => id.Pos);
            byte[] asBytes = _sequence
                .Reverse()
                .SelectMany(p => BitConverter.GetBytes(p))
                .ToArray();
            //MakePositiveValue(asBytes);
            _bigint = new BigInteger(asBytes);
        }

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

        public IEnumerator<int> GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }

        public bool Equals([AllowNull] PosSequence other) => 
            _bigint.Equals(other._bigint);

        public int CompareTo([AllowNull] PosSequence other)
        {
            var thisLen = _sequence.Count();
            var otherLen = other._sequence.Count();
            var maxLen = Math.Max(thisLen, otherLen);
            var paddedThisAsBigInt = GetPrefixAsBigInt(maxLen);
            var paddedOtherAsBigInt = other.GetPrefixAsBigInt(maxLen);
            return BigInteger.Compare(paddedThisAsBigInt, paddedOtherAsBigInt);
        }

        public override int GetHashCode() => 
            HashCode.Combine(_bigint);

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(PosSequence left, PosSequence right) => 
            left.Equals(right);

        public static bool operator !=(PosSequence left, PosSequence right) => 
            !(left == right);

        private PosSequence(BigInteger bigint)
        {
            _bigint = bigint;
            byte[] asBytes = bigint.ToByteArray();

            var padLen = asBytes.Length % sizeof(int) == 0 
                ? 0 
                : sizeof(int) - asBytes.Length % sizeof(int);
            _sequence = asBytes
                .Concat(Enumerable.Repeat<byte>(0, padLen))
                .Select((@byte, i) => (@byte, i))
                .GroupBy(pair => pair.i / sizeof(int), pair => pair.@byte)
                .Reverse()
                .Select(bytesGroup => BitConverter.ToInt32(bytesGroup.ToArray(), 0));       
        }

        private static BigInteger GetRandomBigInt(BigInteger first, BigInteger second)
        {
            if (first > second)
                throw new ArgumentException();

            var random = new Random();
            var diff = second - first;
            var diffBytesCnt = diff.GetByteCount();
            var buf = new byte[diffBytesCnt];
            //var span = new Span<byte>(buf);
            BigInteger result;
            do
            {
                var fillLen = random.Next(diffBytesCnt + 1);
                var slice = buf[..fillLen];
                //span.Clear();
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

        // pads with 0 if prefixLen > seqLen
        private BigInteger GetPrefixAsBigInt(int prefixLen)
        {
            var seqLen = _sequence.Count();
            var padLen = prefixLen <= seqLen ? 0 : prefixLen - seqLen;
            return new BigInteger(_sequence
                .Take(prefixLen)
                .Concat(Enumerable.Repeat<int>(0, padLen))
                .Reverse()
                .SelectMany(i => BitConverter.GetBytes(i))
                .ToArray()
                );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_sequence).GetEnumerator();
        }
    }
}
