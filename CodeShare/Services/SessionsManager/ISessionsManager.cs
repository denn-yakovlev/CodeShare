using System;
using System.Threading.Tasks;

using Session = CodeShare.Model.Entities.Session;
using User = CodeShare.Model.Entities.User;

namespace CodeShare.Services.SessionsManager
{
    /// <summary>
    /// Глобальный сервис управления всеми сессиями совместной работы
    /// </summary>
    public interface ISessionsManager
    {
        Session? GetSessionById(string sessionId);

        Session CreateNewSession();

        Task ConnectToSessionAsync(User user, string sessionId);

        Task DisconnectFromSessionAsync(User user, string sessionId);

        event Func<object, SessionConnectionEventArgs, Task> Connected; 

        event Func<object, SessionDisconnectionEventArgs, Task> Disconnected; 
    }
}
