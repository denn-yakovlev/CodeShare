using CodeShare.Model.DTOs;
using System.Collections;
using System.Collections.Generic;

namespace CodeShare.Services.CollabManager
{
    public class CollaboratorInfo
    {
        public string UserName { get; set; }

        public ICollection<Permission> PermissionsInProject { get; set; }
    
        public ICollection<string> ConnectionIds { get; set; }

        public bool IsAnonymous { get; set; }
    }
}