using System;
using System.Security.Principal;

namespace CodeShare.Utils
{
    public class ConnToProjectEventArgs : EventArgs
    {
        public string ProjectId { get; set; }

        public IPrincipal User { get; set; }

        public string ConnectionId { get; set; }
    }
}
