using CodeShare.Utils;
using CodeShare.Utils.Logoot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.Model.Entities
{
    public class LogootDocument : ICloneable
    {
        public Solution Solution => new Solution()
        {
            Language = SolutionLanguage,
            SourceCode = SolutionSourceCode
        };

        public ProgrammingLanguage SolutionLanguage { get; set; } = ProgrammingLanguage.C;

        public LogootDocument()
        {
            _atomsSyncRoot = ((ICollection) _atoms).SyncRoot;
        }

        public LogootDocument(string text) : this()
        {
            var atoms = PositionId
                .Generate((PositionId.Min, PositionId.Max), text.Length, 0, 0)
                .Zip(text, (pos, @char) => new LogootAtom(pos, @char));
            InsertRange(atoms);
        }

        private LogootDocument(List<LogootAtom> atoms)
        {
            _atoms = atoms;
            _atomsSyncRoot = ((ICollection)_atoms).SyncRoot;
        }

        public object Clone()
        {
            return new LogootDocument(new List<LogootAtom>(_atoms));
        }

        private string SolutionSourceCode => new string(_atoms
            .Skip(1)
            .SkipLast(1)
            .Select(atom => atom.Char)
            .ToArray()
            );

        private List<LogootAtom> _atoms = new List<LogootAtom>
        {
            LogootAtom.First,
            LogootAtom.Last
        };

        private readonly object _atomsSyncRoot;

        public void InsertRange(IEnumerable<LogootAtom> atomsToInsert)
        {
            if (atomsToInsert.Any())
            {
                var firstAtom = atomsToInsert.First();
                var lessThanAtomToInsertCount = _atoms.TakeWhile(atom => atom.Id < firstAtom.Id).Count();
                lock (_atomsSyncRoot)
                    _atoms.InsertRange(lessThanAtomToInsertCount, atomsToInsert);
            }
            else
            {
                throw new ArgumentException("Argument must be non-empty enumerable", nameof(atomsToInsert));
            }
        }

        public void Insert(LogootAtom atomToInsert)
        {
            EnsureValidIdToInsert(atomToInsert.Id);
            var lessThanAtomToInsertCount = _atoms.TakeWhile(atom => atom.Id < atomToInsert.Id).Count();
            lock (_atomsSyncRoot)
                _atoms.Insert(lessThanAtomToInsertCount, atomToInsert);
        }

        public void Remove(LogootAtom atomToRemove)
        {
            EnsureValidIdToRemove(atomToRemove.Id);
            lock (_atomsSyncRoot)
            {
                _atoms.Remove(atomToRemove);
            }
        }

        public void RemoveRange(IEnumerable<LogootAtom> atomsToRemove)
        {
            EnsureValidIdToRemove(atomsToRemove.First().Id);
            foreach (var atom in atomsToRemove)
            {
                lock (_atomsSyncRoot)
                {
                    _atoms.Remove(atom);
                }
            }
            
        }

        public LogootAtom GetAtomByOffset(int offset)
        {
            lock (_atomsSyncRoot)
            {
                return _atoms[offset + 1];
            }
        }

        public IEnumerable<LogootAtom> GetAtomsArray(int offset, int length)
        {
            var atoms = _atoms.Skip(1).SkipLast(1).Skip(offset).Take(length);
            lock (_atomsSyncRoot)
            {
                return atoms.ToArray();
            }
        }


        private static void EnsureValidIdToRemove(PositionId idToRemove)
        {
            if (idToRemove == PositionId.Min)
                throw new ArgumentException($"cannot remove first atom");
            if (idToRemove == PositionId.Max)
                throw new ArgumentException($"cannot remove last atom");
        }

        private void EnsureValidIdToInsert(PositionId id)
        {
            if (id <= PositionId.Min)
                throw new ArgumentException($"inserted position id must be greater than {nameof(PositionId.Min)}");
            if (id >= PositionId.Max)
                throw new ArgumentException($"inserted position id must be less than {nameof(PositionId.Max)}");
        }
    }
}
