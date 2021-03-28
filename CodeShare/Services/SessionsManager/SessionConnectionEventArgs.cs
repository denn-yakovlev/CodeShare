using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Services.SessionsManager
{
    public class SessionConnectionEventArgs : SessionEventArgs
    {
        /// <summary>
        /// Является ли данного подключение первым для данной сессии
        /// </summary>
        public bool IsFirst { get; set; }
    }
}
