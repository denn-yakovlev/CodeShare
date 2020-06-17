using CodeShare.Models;
using CodeShare.Services.TextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.CollabManager
{
    public class CollabSessionInfo
    {
        public ICollection<CollaboratorInfo> Collaborators { get; set; }

        public Project ActiveProject { get; set; }

        public ITextEditorService TextEditor { get; set; }
    }
}
