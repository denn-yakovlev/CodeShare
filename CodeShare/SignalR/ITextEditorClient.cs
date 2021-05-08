using CodeShare.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.SignalR
{
    public interface ITextEditorClient
    {
        Task InsertAsync(LogootAtom atom);

        Task DeleteAsync(LogootAtom atom);
    }
}
