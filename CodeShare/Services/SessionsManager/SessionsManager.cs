using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using CodeShare.Services.DatabaseInteractor;

using Session = CodeShare.Model.Entities.Session;
using User = CodeShare.Model.Entities.User;
using TaskEntity = CodeShare.Model.Entities.Task;
using SessionDTO = CodeShare.Model.DTOs.Session;

using System.Threading;
using AutoMapper;

namespace CodeShare.Services.SessionsManager
{
    public class SessionsManager : ISessionsManager
    {
        private ConcurrentDictionary<string, Session> _activeSessions = 
            new ConcurrentDictionary<string, Session>();

        private IDatabaseInteractor _dbInteractor;
        private IMapper _automapper;

        public SessionsManager(IDatabaseInteractor dbInteractor, IMapper mapper)
        {
            _dbInteractor = dbInteractor;
            _automapper = mapper;
            Connected += HandleFirstConnected;
            Disconnected += HandleLastDisconnected;
        }

        public async Task ConnectToSession(User user, string sessionId)
        {
            //if (_activeSessions.ContainsKey(sessionId))
            //    session = _activeSessions[sessionId];
            //else
            //{
            //    session = _automapper.Map<Session>(
            //        await _dbInteractor.Sessions.ReadAsync(sessionId)
            //        );
            //    _activeSessions.TryAdd(sessionId, session);
            //}
            //_activeSessions[sessionId].Collaborators.Add(user);
            await OnConnectedAsync(
                this,
                new SessionConnectionEventArgs
                {
                    SessionId = sessionId,
                    User = user,
                    IsFirst = !_activeSessions.ContainsKey(sessionId)
                });
            var session = _activeSessions[sessionId];
            session.Collaborators.Add(user);
        }
            

        public Session CreateNewSession()
        {
            var newSession = new Session
            {
                Id = Guid.NewGuid().ToString(),
                CurrentTask = null,
            };
            _activeSessions.TryAdd(newSession.Id, newSession);
            return newSession;
        }

        public async Task DisconnectFromSession(User user, string sessionId)
        {
            var collaborators = _activeSessions[sessionId].Collaborators;
            collaborators.Remove(user);
            await OnDisconnectedAsync(
                this,
                new SessionDisconnectionEventArgs
                {
                    SessionId = sessionId,
                    User = user,
                    IsLast = collaborators.Count == 0
                });
        }

        public Session? GetSessionById(string sessionId)
        {
            bool hasSession = _activeSessions.TryGetValue(sessionId, out Session session);
            if (hasSession)
                return session;
            return null;
        }

        public event Func<object, SessionConnectionEventArgs, Task> Connected;
        public event Func<object, SessionDisconnectionEventArgs, Task> Disconnected;

        private async Task OnConnectedAsync(object sender, SessionConnectionEventArgs ea) => 
            await (Volatile.Read(ref Connected)?.Invoke(sender, ea) ?? Task.CompletedTask);

        private async Task OnDisconnectedAsync(object sender, SessionDisconnectionEventArgs ea) =>
            await (Volatile.Read(ref Disconnected)?.Invoke(sender, ea) ?? Task.CompletedTask);


        private async Task HandleLastDisconnected(object sender, SessionDisconnectionEventArgs ea)
        {
            if (ea.IsLast)
            {
                var session = _activeSessions[ea.SessionId];
                var sessionDto = _automapper.Map<Session, SessionDTO>(session);
                await _dbInteractor.Sessions.UpdateAsync(ea.SessionId, sessionDto);
                _activeSessions.TryRemove(ea.SessionId, out _);
            }
        }

        private async Task HandleFirstConnected(object sender, SessionConnectionEventArgs ea)
        {
            if (ea.IsFirst)
            {
                var sessionDto = await _dbInteractor.Sessions.ReadAsync(ea.SessionId);
                var session = _automapper.Map<SessionDTO, Session>(sessionDto);
                _activeSessions.TryAdd(ea.SessionId, session);
            }
        }
    }
}
