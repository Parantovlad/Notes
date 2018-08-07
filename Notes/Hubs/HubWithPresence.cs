using Microsoft.AspNetCore.SignalR;
using Notes.Models;
using Notes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Hubs
{
    public abstract class HubWithPresence : Hub
    {
        private IUserTracker _userTracker;

        public HubWithPresence(IUserTracker userTracker)
        {
            _userTracker = userTracker;
            _userTracker.UserJoined += OnUsersJoined;
            _userTracker.UserLeft += OnUsersLeft;
        }

        public virtual async void OnUsersJoined(UserDetails user) { }

        public virtual async void OnUsersLeft(UserDetails user) { }
    }
}
