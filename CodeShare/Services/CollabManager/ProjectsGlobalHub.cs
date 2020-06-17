using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CodeShare.Services.CollabManager
{
    public class ProjectsGlobalHub : Hub
    {
        ICollabManager collabManager;

        IHttpContextAccessor httpContextAccessor;

        // ILogger logger;

        public ProjectsGlobalHub(ICollabManager mgr, IHttpContextAccessor httpContextAccessor)
        {
            collabManager = mgr;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task ConnectToProject()
        {
            //var routeSegments = new Uri(httpContextAccessor.HttpContext.Request.RouteValues[]).Segments;
            var projectId = httpContextAccessor.HttpContext.Request.RouteValues["projectId"] as string;
            var user = httpContextAccessor.HttpContext.User;
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
            Console.WriteLine(
                $"{user.Identity.Name}:{Context.ConnectionId} is connected to {projectId}"
                );
            await collabManager.AssignToProjectAsync(projectId, user, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var projectId = httpContextAccessor.HttpContext.Request.RouteValues["projectId"] as string;
            var user = httpContextAccessor.HttpContext.User;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId);
            Console.WriteLine(
                $"{user.Identity.Name}:{Context.ConnectionId} is disconnected from {projectId}"
                );
            collabManager.RemoveFromProject(projectId, user, Context.ConnectionId);
        }
    }
}
