using Microsoft.AspNetCore.SignalR;
using Notes.Models;
using Notes.Services;
using Notes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Hubs
{
    public class ChatHub : HubWithPresence
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService, IUserTracker userTracker)
            : base(userTracker)
        {
            _chatService = chatService;
        }

        public async Task AddMessage(string message)
        {
            var username = Context.User.Identity.Name;
            var chatMessage = await _chatService.CreateNewMessage(username, message);
            
            await Clients.All.SendAsync("MessageAdded", chatMessage);
        }

        public override async void OnUsersJoined(UserDetails user)
        {
            await Clients.All.SendAsync("UsersJoined", user);
        }

        public override async void OnUsersLeft(UserDetails user)
        {
            await Clients.All.SendAsync("UsersLeft", user);
        }
    }
}
