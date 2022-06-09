using Microsoft.AspNetCore.SignalR;
using SnakeVS.Shared;
using SnakeVS.Shared.Response;

namespace SnakeVS.Server.Hubs
{
    public class RoomsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Global.ListingRoomsProxy = Clients.All;
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Global.ListingRoomsProxy = Clients.All;
            return base.OnDisconnectedAsync(exception);
        }

        public async Task CreateRoom(string name)
        {
            if (String.IsNullOrEmpty(name.Trim())) return;
            if (Global.Rooms.Any(p => p.Name.ToUpper() == name.ToUpper())) return;
            var room = new Room() { Guid = Guid.NewGuid(), Name = name.Trim() };
            Global.Rooms.Add(room);
            await Clients.All.SendAsync("GetRooms", Global.Rooms.Select(p => new ListedRoom(p)).ToArray());
        }

        public async Task ListRooms()
        {
            await Clients.Caller.SendAsync("GetRooms", Global.Rooms.Select(p => new ListedRoom(p)).ToArray());
        }

        public async Task DeleteRoom(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            if (String.IsNullOrEmpty(room.BlueName) && String.IsNullOrEmpty(room.RedName))
            {
                Global.Rooms.Remove(room);
                await Clients.All.SendAsync("GetRooms", Global.Rooms.Select(p => new ListedRoom(p)).ToArray());
            }
        }
    }
}
