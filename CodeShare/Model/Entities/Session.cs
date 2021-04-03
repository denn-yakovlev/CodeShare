using CodeShare.Services.TextEditor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeShare.Model.Entities
{
    // основная сущность - сессия совместной работы
    public class Session : IEquatable<Session>
    {
        public string Id { get; set; }

        public Task? CurrentTask { get; set; }

        public ICollection<User> Collaborators { get; set; } = new List<User>();

        public ITextEditor EditorInstance { get; set; }

        public bool Equals([AllowNull] Session other) => 
            other != null && other.Id == Id;
    }
}
