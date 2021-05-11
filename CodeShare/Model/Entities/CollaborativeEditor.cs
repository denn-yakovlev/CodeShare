using CodeShare.Model.Entities;
using CodeShare.Utils;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class CollaborativeEditor : ICloneable
    {
        public Solution Solution => new Solution()
        {
            Language = SolutionLanguage,
            SourceCode = SolutionSourceCode
        };

        public ProgrammingLanguage SolutionLanguage { get; set; } = ProgrammingLanguage.C;

        public CollaborativeEditor()
        {
            _atomsSyncRoot = ((ICollection) _atoms).SyncRoot;
        }

        public CollaborativeEditor(string text) : this()
        {
            var atoms = LogootId
                .GenerateBetween(LogootId.Min, LogootId.Max, text.Length, 0, 0)
                .Zip(text)
                .Select(pair => new LogootAtom(pair.First, pair.Second));
            InsertRange(atoms);
        }

        private CollaborativeEditor(List<LogootAtom> atoms)
        {
            _atoms = atoms;
            _atomsSyncRoot = ((ICollection)_atoms).SyncRoot;
        }

        public object Clone()
        {
            return new CollaborativeEditor(new List<LogootAtom>(_atoms));
        }

        private string SolutionSourceCode => new string(_atoms
            .Skip(1)
            .SkipLast(1)
            .Select(atom => atom.Char)
            .Cast<char>()
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

        //private void EnsureValidIdToInsertAfter(LogootId idToInsertAfter)
        //{
        //    var ids = _atoms.Select(atom => atom.Id);
        //    if (!ids.Contains(idToInsertAfter))
        //        throw new ArgumentException($"{nameof(idToInsertAfter)} doesn't exist");
        //    if (idToInsertAfter == LogootId.Max)
        //        throw new ArgumentException($"cannot insert after last atom");
        //}

        private static void EnsureValidIdToRemove(LogootId idToRemove)
        {
            if (idToRemove == LogootId.Min)
                throw new ArgumentException($"cannot remove first atom");
            if (idToRemove == LogootId.Max)
                throw new ArgumentException($"cannot remove last atom");
        }
    }
}
