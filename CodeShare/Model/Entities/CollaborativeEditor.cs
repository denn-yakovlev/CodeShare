using CodeShare.Model.Entities;
using CodeShare.Utils;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Model.Entities
{
    public class CollaborativeEditor
    {
        public Solution Solution => new Solution()
        {
            Language = SolutionLanguage,
            SourceCode = SolutionSourceCode
        };

        public ProgrammingLanguage SolutionLanguage { get; set; } = ProgrammingLanguage.C;

        //private IHubContext<TextEditorHub> _hubContext;

        //public CollaborativeEditor(IHubContext<TextEditorHub> hubContext)
        //{
        //    _hubContext = hubContext;
        //}

        public CollaborativeEditor()
        {
            
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

        private const string hubClientInsertMethodName = nameof(InsertAfter);
        private const string hubClientDeleteMethodName = nameof(Remove);

        public void InsertAfter(LogootId idToInsertAfter, LogootAtom atom)
        {
            EnsureValidIdToInsertAfter(idToInsertAfter);
            int i;
            lock (((ICollection)_atoms).SyncRoot)
            {
                for (i = 0; i < _atoms.Count; i++)
                {
                    if (_atoms[i].Id == idToInsertAfter)
                        break;
                }
                _atoms.Insert(i + 1, atom);            }
        }

        public void Remove(LogootId idToRemove)
        {
            EnsureValidIdToRemove(idToRemove);
            _atoms.RemoveAll(atom => atom.Id == idToRemove);
        }

        private void EnsureValidIdToInsertAfter(LogootId idToInsertAfter)
        {
            var ids = _atoms.Select(atom => atom.Id);
            if (!ids.Contains(idToInsertAfter))
                throw new ArgumentException($"{nameof(idToInsertAfter)} doesn't exist");
            if (idToInsertAfter == LogootId.Max)
                throw new ArgumentException($"cannot insert after last atom");
        }

        private static void EnsureValidIdToRemove(LogootId idToRemove)
        {
            if (idToRemove == LogootId.Min)
                throw new ArgumentException($"cannot remove first atom");
            if (idToRemove == LogootId.Max)
                throw new ArgumentException($"cannot remove last atom");
        }
    }
}
