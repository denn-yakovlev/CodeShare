using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.SessionsManager
{
    public class SessionDisconnectionEventArgs : SessionEventArgs
    {
        /// <summary>
        /// Является ли данного подключение последним для данной сессии
        /// </summary>
        public bool IsLast { get; set; }
    }
}
