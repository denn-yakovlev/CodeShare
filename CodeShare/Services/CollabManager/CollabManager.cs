using CodeShare.Model.DTOs;
using CodeShare.Services.DatabaseInteractor;
using CodeShare.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CodeShare.Services.CollabManager
{
    public class CollabManager : ICollabManager
    {
        private IDatabaseInteractor dbInteractor;

        public CollabManager(IDatabaseInteractor dbInteractor)
        {
            this.dbInteractor = dbInteractor;
        }

        public event Func<object, ConnToProjectEventArgs, Task> Connected;
        public event Func<object, ConnToProjectEventArgs, Task> Disconnected;

        private IDictionary<string, CollabSessionInfo> activeSessions = 
            new Dictionary<string, CollabSessionInfo>();

        public async Task AssignToProjectAsync(string projectId, IPrincipal user, string connectionId)
        {
            if (!activeSessions.ContainsKey(projectId))
                await AddCollabSessionAsync(projectId);

            CollaboratorInfo collaborator = null;

            if (!user.Identity.IsAuthenticated)
                AddCollaborator(projectId, CreateAnonymousCollaborator(connectionId));            
            else
            {
                collaborator = activeSessions[projectId].Collaborators.SingleOrDefault(
                    clb => clb.UserName == user.Identity.Name
                    );
                if (collaborator == null)
                    AddCollaborator(projectId, CreateAuthenticatedCollaborator(user, connectionId));
                else
                    collaborator.ConnectionIds.Add(connectionId);
            }

            Volatile.Read(ref Connected)?.Invoke(
                this, new ConnToProjectEventArgs
                {
                    ProjectId = projectId,
                    ConnectionId = connectionId,
                    User = user
                });
        }

        private void AddCollaborator(string projectId, CollaboratorInfo clbInfo)
        {
            activeSessions[projectId].Collaborators.Add(clbInfo);
        }

        private CollaboratorInfo CreateAuthenticatedCollaborator(IPrincipal user, string connectionId) => new CollaboratorInfo
        {
            UserName = user.Identity.Name,
            ConnectionIds = new List<string> { connectionId },
            IsAnonymous = false,
            PermissionsInProject = null
        };

        private CollaboratorInfo CreateAnonymousCollaborator(string connectionId) => new CollaboratorInfo
        {
            UserName = $"__Anonymous#{connectionId}",
            IsAnonymous = true,
            ConnectionIds = new List<string> { connectionId },
            PermissionsInProject = null
        };

        private async Task AddCollabSessionAsync(string projectId)
        {
            var project =  await dbInteractor.Projects.ReadAsync(projectId);
            activeSessions.Add(projectId,
                new CollabSessionInfo
                {
                    ActiveProject = project,
                    Collaborators = new List<CollaboratorInfo>(),
                }
            );
        }

        public IEnumerable<CollaboratorInfo> GetProjectCollaborators(string projectId) => 
            activeSessions[projectId].Collaborators;

        public void RemoveFromProject(string projectId, IPrincipal user, string connectionId)
        {
            if (activeSessions.ContainsKey(projectId))
            {
                CollaboratorInfo collaboratorToRemove;
                if (user.Identity.IsAuthenticated)
                    collaboratorToRemove = activeSessions[projectId].Collaborators.SingleOrDefault(
                        clb => clb.UserName == user.Identity.Name
                        );
                else
                    collaboratorToRemove = activeSessions[projectId].Collaborators.SingleOrDefault(
                        anonClb => anonClb.ConnectionIds.Contains(connectionId)
                        );
                activeSessions[projectId].Collaborators.Remove(collaboratorToRemove);
                Volatile.Read(ref Disconnected)?.Invoke(
                    this, new ConnToProjectEventArgs
                    {
                        ProjectId = projectId,
                        ConnectionId = connectionId,
                        User = user
                    });
            }
        }

        public Project GetProjectById(string projectId) => activeSessions[projectId].ActiveProject;
    }
}
