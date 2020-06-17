using CodeShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CodeShare.Services
{
    public class ConnToProjectEventArgs : EventArgs
    {
        public string ProjectId { get; set; }

        public IPrincipal User { get; set; }

        public string ConnectionId { get; set; }
    }
}
