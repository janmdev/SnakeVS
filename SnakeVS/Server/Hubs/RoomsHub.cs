using Microsoft.AspNetCore.SignalR;
using SnakeVS.Shared;
using SnakeVS.Shared.Response;

namespace SnakeVS.Server.Hubs
{
    public class RoomsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task CreateRoom(string name)
        {
            var room = new Room() { Guid = Guid.NewGuid(), Name = name };
            Global.Rooms.Add(room);
            await Clients.All.SendAsync("GetRooms", Global.Rooms.Select(p => new ListedRoom(p)).ToArray());
        }

        public async Task ListRooms()
        {
            await Clients.Caller.SendAsync("GetRooms", Global.Rooms.Select(p => new ListedRoom(p)).ToArray());
        }
    }
}
