using System;
using User = CodeShare.Model.Entities.User;
namespace CodeShare.Services.SessionsManager
{
    public class SessionEventArgs : EventArgs
    {
        public string SessionId { get; set; }

        public User User { get; set; }
    }
}
