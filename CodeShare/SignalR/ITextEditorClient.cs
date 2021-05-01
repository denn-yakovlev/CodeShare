using CodeShare.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.SignalR
{
    public interface ITextEditorClient
    {
        void InsertAfter(LogootId idToInsertAfter, LogootAtom atom);

        void Delete(LogootId idToDelete);
    }
}
