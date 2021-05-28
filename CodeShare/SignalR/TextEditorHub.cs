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
    public class TextEditorHub : Hub//Hub<ITextEditorClient>
    {
        private ISessionsManager _sessionsMgr;

        private ILogger<TextEditorHub> _logger;

        public TextEditorHub(ISessionsManager sessionsMgr, ILogger<TextEditorHub> logger)
        {
            _sessionsMgr = sessionsMgr;
            _logger = logger;
        }

        public async Task BroadcastInsertionAsync(string sessionId, LogootAtom atom)
        {
            var editor = _sessionsMgr.GetSessionById(sessionId)?.LogootDocument;
            editor?.Insert(atom);
            await Clients.OthersInGroup(sessionId).SendAsync("Insert", atom);
            //await Clients.OthersInGroup(sessionId).InsertAsync(atom);
        }

        public async Task BroadcastDeletionAsync(string sessionId, LogootAtom atom)
        {
            var editor = _sessionsMgr.GetSessionById(sessionId)?.LogootDocument;
            editor?.Remove(atom);
            await Clients.OthersInGroup(sessionId).SendAsync("Delete", atom);
            //await Clients.OthersInGroup(sessionId).DeleteAsync(atom);
        }

        public async Task BroadcastRangeInsertionAsync(string sessionId, IEnumerable<LogootAtom> atoms)
        {
            var editor = _sessionsMgr.GetSessionById(sessionId)?.LogootDocument;
            editor?.InsertRange(atoms);
            await Clients.OthersInGroup(sessionId).SendAsync("InsertRange", atoms);
            //await Clients.OthersInGroup(sessionId).InsertAsync(atom);
        }

        public async Task BroadcastRangeDeletionAsync(string sessionId, IEnumerable<LogootAtom> atoms)
        {
            var editor = _sessionsMgr.GetSessionById(sessionId)?.LogootDocument;
            editor?.RemoveRange(atoms);
            await Clients.OthersInGroup(sessionId).SendAsync("DeleteRange", atoms);
            //await Clients.OthersInGroup(sessionId).DeleteAsync(atom);
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
