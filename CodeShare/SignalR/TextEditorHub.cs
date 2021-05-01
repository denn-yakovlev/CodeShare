using CodeShare.Services.SessionsManager;
using CodeShare.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.SignalR
{
    public class TextEditorHub : Hub<ITextEditorClient>
    {
        private ISessionsManager _sessionsMgr;

        private ILogger<TextEditorHub> _logger;

        public TextEditorHub(ISessionsManager sessionsMgr, ILogger<TextEditorHub> logger)
        {
            _sessionsMgr = sessionsMgr;
            _logger = logger;
        }

        public void BroadcastInsertionAsync(string sessionId, LogootId idToInsertAfter, LogootAtom atom)
        {
            var editor = _sessionsMgr.GetSessionById(sessionId)?.EditorInstance;
            editor?.InsertAfter(idToInsertAfter, atom);
            Clients.OthersInGroup(sessionId).InsertAfter(idToInsertAfter, atom);
        }

        public void BroadcastDeletionAsync(string sessionId, LogootId idToRemove)
        {
            var editor = _sessionsMgr.GetSessionById(sessionId)?.EditorInstance;
            editor?.Remove(idToRemove);
            Clients.OthersInGroup(sessionId).Delete(idToRemove);
        }

        public override async Task OnConnectedAsync()
        {
            var sessionId = Context.GetHttpContext().Request.RouteValues["sessionId"].ToString();
            var connectionId = Context.ConnectionId;
            _logger.LogInformation($"Connected to {nameof(TextEditorHub)}: connection={connectionId}, session={sessionId}");
            await Groups.AddToGroupAsync(connectionId, sessionId);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var sessionId = Context.GetHttpContext().Request.RouteValues["sessionId"].ToString();
            var connectionId = Context.ConnectionId;
            _logger.LogInformation($"Disconnected from {nameof(TextEditorHub)}: connection={connectionId}, session={sessionId}");
            await Groups.RemoveFromGroupAsync(connectionId, sessionId);
        }
    }
}
