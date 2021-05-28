using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using CodeShare.Services.DatabaseInteractor;
using AutoMapper;
using Session = CodeShare.Model.Entities.Session;
using User = CodeShare.Model.Entities.User;
using SessionDTO = CodeShare.Model.DTOs.Session;

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
        }

        public async Task<Session> ConnectToSessionAsync(User user, string sessionId)
        {
            var session = await GetOrLoadSessionAsync(sessionId);
            session.Collaborators.Add(user);
            await session.OnConnected();
            return session;
        }

        private async Task<Session> GetOrLoadSessionAsync(string sessionId)
        {
            Session session;
            if (_activeSessions.ContainsKey(sessionId))
            {
                session = _activeSessions[sessionId];
            }
            else
            {
                var sessionDto = await _dbInteractor.Sessions.ReadAsync(sessionId);
                session = _automapper.Map<SessionDTO, Session>(sessionDto);
                _activeSessions.TryAdd(sessionId, session);
            }
            return session;
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

        public async Task<Session> DisconnectFromSessionAsync(User user, string sessionId)
        {
            var session = _activeSessions[sessionId];
            var collaborators = session.Collaborators;
            collaborators.Remove(user);
            if (collaborators.Count == 0)
            {
                var sessionDto = _automapper.Map<Session, SessionDTO>(session);
                await _dbInteractor.Sessions.UpdateAsync(sessionId, sessionDto);
                _activeSessions.TryRemove(sessionId, out _);
            }
            await session.OnDisconnected();
            return session;
        }

        public Session GetSessionById(string sessionId) => _activeSessions[sessionId];
    }
}
