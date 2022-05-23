using Microsoft.AspNetCore.SignalR;
using MessagePack;
using SnakeVS.Shared;

namespace SnakeVS.Server.Hubs
{
    public class GameHub : Hub
    {
        public List<Room> Rooms = new List<Room>();
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task CreateRoom(Guid guid, string name, int x)
        {
            var room = new Room() { Guid = guid, Name = name };
            Rooms.Add(room);
            await Clients.All.SendAsync("AddRoom", room);
        }
    }
}
