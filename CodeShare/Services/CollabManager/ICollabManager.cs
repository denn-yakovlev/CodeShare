using CodeShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CodeShare.Services.CollabManager
{
    public interface ICollabManager
    {
        IEnumerable<CollaboratorInfo> GetProjectCollaborators(string projectId);

        Project GetProjectById(string projectId);

        Task AssignToProjectAsync(string projectId, IPrincipal user, string connectionId);

        void RemoveFromProject(string projectId, IPrincipal user, string connectionId);

        event Func<object, ConnToProjectEventArgs, Task> Connected;

        event Func<object, ConnToProjectEventArgs, Task> Disconnected;
    
        
    }
}
