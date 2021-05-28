using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CodeShare.Model.Entities
{
    // основная сущность - сессия совместной работы
    public class Session : ISessionState
    {
        public string Id { get; set; } = string.Empty;

        public Task? CurrentTask { get; set; }

        public ICollection<User> Collaborators { get; set; } = new List<User>();

        public LogootDocument? LogootDocument { get; set; }

        public bool Equals([AllowNull] ISessionState other) =>
            other != null && other.Id == Id;

        public event Func<ISessionState, System.Threading.Tasks.Task> Connected;
        public event Func<ISessionState, System.Threading.Tasks.Task> Disconnected;

        public System.Threading.Tasks.Task OnConnected() => OnEvent(Connected);

        public System.Threading.Tasks.Task OnDisconnected() => OnEvent(Disconnected);

        private System.Threading.Tasks.Task OnEvent(Func<ISessionState, System.Threading.Tasks.Task> ev)
        {
            var handlerTasks = (ev
                ?.GetInvocationList() ?? new Delegate[0])
                .Select(del =>
                    ((Func<ISessionState, System.Threading.Tasks.Task>)del).Invoke(this)
                )
                .ToArray();
            return System.Threading.Tasks.Task.WhenAll(handlerTasks);
        }

        public static ISessionState Empty { get; } = new Session
        {
            Id = string.Empty,
            CurrentTask = null,
            Collaborators = new List<User>(),
            LogootDocument = null
        };
    }
}
