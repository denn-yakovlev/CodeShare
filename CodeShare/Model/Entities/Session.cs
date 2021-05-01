using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeShare.Model.Entities
{
    // основная сущность - сессия совместной работы
    public class Session : IEquatable<Session>
    {
        public string Id { get; set; } = string.Empty;

        public Task? CurrentTask { get; set; }

        public ICollection<User> Collaborators { get; set; } = new List<User>();

        public CollaborativeEditor? EditorInstance { get; set; } 

        public bool Equals([AllowNull] Session other) => 
            other != null && other.Id == Id;

        public static Session Empty { get; } = new Session
        {
            Id = string.Empty,
            CurrentTask = null,
            Collaborators = new List<User>(),
            EditorInstance = null
        };
    }
}
