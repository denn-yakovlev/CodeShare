using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CodeShare.Model.Entities
{
    public interface ISessionState : IEquatable<ISessionState>
    {
        ICollection<User> Collaborators { get; set; }
        Task? CurrentTask { get; set; }
        CollaborativeEditor? EditorInstance { get; set; }
        string Id { get; set; }

        event Func<ISessionState, System.Threading.Tasks.Task> Connected;
        event Func<ISessionState, System.Threading.Tasks.Task> Disconnected;
    }
}