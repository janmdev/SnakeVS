using Microsoft.AspNetCore.SignalR;
using MessagePack;
using SnakeVS.Shared;
using SnakeVS.Shared.Response;

namespace SnakeVS.Server.Hubs
{
    public class GameHub : Hub
    {


        private Guid getRoomGuidByClient(string client)
        {
            return Global.RoomClients.FirstOrDefault(p => p.Value.Contains(client)).Key;
        }
        public override async Task OnConnectedAsync()
        {
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var roomGuid = getRoomGuidByClient(Context.ConnectionId);
            await LeaveRoom(roomGuid);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task LeaveRoom_(Guid roomGuid, string caller)
        {
            if (Global.RoomClients[roomGuid].RedClient == caller)
            {
                Global.RoomClients[roomGuid].RedClient = null;
                Global.RoomClients[roomGuid].RedName = null;
            }
            if (Global.RoomClients[roomGuid].BlueClient == caller)
            {
                Global.RoomClients[roomGuid].BlueClient = null;
                Global.RoomClients[roomGuid].BlueName = null;
            }

            await Groups.RemoveFromGroupAsync(caller, roomGuid.ToString());
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            if (room.State == GameState.Playing) room.State = GameState.Paused;
        }

        public async Task LeaveRoom(Guid roomGuid)
        {
            await LeaveRoom_(roomGuid, Context.ConnectionId);
        }

        public async Task JoinGame(Guid roomGuid, string userName, bool blue)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            if (!Global.RoomClients.ContainsKey(room.Guid))
            {
                Global.RoomClients.Add(room.Guid, new PlayerConnection());
            }
            else
            {
                if ((blue && Global.RoomClients[room.Guid].BlueClient != null &&
                    Global.RoomClients[room.Guid].BlueClient != Context.ConnectionId) ||
                    (!blue && Global.RoomClients[room.Guid].RedClient != null &&
                     Global.RoomClients[room.Guid].RedClient != Context.ConnectionId))
                {
                    await Clients.Caller.SendAsync("FullRoom");
                    return;
                }
            }

            
            if (blue)
            {
                Global.RoomClients[roomGuid].BlueClient = Context.ConnectionId;
                room.BlueName = userName;
                Global.RoomClients[roomGuid].BlueName = userName;
            }
            else
            {
                Global.RoomClients[roomGuid].RedClient = Context.ConnectionId;
                room.RedName = userName;
                Global.RoomClients[roomGuid].RedName = userName;
            }

            await Global.ListingRoomsProxy.SendAsync("GetRooms", Global.Rooms.Select(p => new ListedRoom(p)).ToArray());
            await Groups.AddToGroupAsync(Context.ConnectionId, roomGuid.ToString());
            await Clients.Group(roomGuid.ToString()).SendAsync("UpdateRoom", room);
        }

        public async Task GetGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomGuid.ToString());
            await Clients.Caller.SendAsync("UpdateRoom", room);
        }

        public async Task SendMove(Guid roomGuid,Direction direction)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            if (Global.RoomClients[roomGuid].BlueClient == Context.ConnectionId)
            {
                room.BlueSnake.QueueDirection(direction);
            }

            if (Global.RoomClients[roomGuid].RedClient == Context.ConnectionId)
            {
                room.RedSnake.QueueDirection(direction);
            }
        }

        public async Task StartGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            if (String.IsNullOrEmpty(room.BlueName) || String.IsNullOrEmpty(room.RedName)) return;// send err
            room.State = GameState.Playing;
            gameRunner(room, Clients.Group(roomGuid.ToString())).Start();

        }


        public async Task ResetGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            room.Reset();
            await Clients.Group(roomGuid.ToString()).SendAsync("UpdateRoom", room);
        }

        public async Task PauseGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            room.State = GameState.Paused;
        }

        public async Task ResumeGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            room.State = GameState.Playing;
        }

        private async Task gameRunner(Room room, IClientProxy client)
        {
            _ = FoodSpawner(room);
            while (true)
            {
                while (room.State == GameState.Paused) 
                    await Task.Delay(250);
                await room.BlueSnake.Move();
                await room.RedSnake.Move();
                consume(room);
                room.State = checkGame(room);
                await client.SendAsync("UpdateRoom", room);
                if (room.State != GameState.Playing) break;
                await Task.Delay(250);
            }
        }

        private async Task FoodSpawner(Room room)
        {
            while (true)
            {
                if (room.State == GameState.Playing)
                {
                    room.SpawnFood();
                    await Task.Delay(3000);
                    
                } else if (room.State != GameState.Paused && room.State != GameState.Init)
                {
                    break;
                }
            }
        }


        //TODO SPRAWDZ WARUNKI
        private GameState checkGame(Room room)
        {
            if ((room.BlueSnake.Nodes.Where(p => !p.IsHead).Any(p =>
                    p.PositionX == room.BlueSnake.Head.PositionX && p.PositionY == room.BlueSnake.Head.PositionY)) ||
                (room.RedSnake.Nodes.Any(p =>
                    p.PositionX == room.BlueSnake.Head.PositionX && p.PositionY == room.BlueSnake.Head.PositionY)) ||
                room.BlueSnake.Head.PositionX < 0 || room.BlueSnake.Head.PositionY < 0 ||
                room.BlueSnake.Head.PositionX > 10 || room.BlueSnake.Head.PositionY > 10)
            {
                return GameState.BlueWon;
            }

            if ((room.RedSnake.Nodes.Where(p => !p.IsHead).Any(p =>
                    p.PositionX == room.RedSnake.Head.PositionX && p.PositionY == room.RedSnake.Head.PositionY)) ||
                (room.BlueSnake.Nodes.Any(p =>
                    p.PositionX == room.RedSnake.Head.PositionX && p.PositionY == room.RedSnake.Head.PositionY)) ||
                room.RedSnake.Head.PositionX < 0 || room.RedSnake.Head.PositionY < 0 ||
                room.RedSnake.Head.PositionX > 10 || room.RedSnake.Head.PositionY > 10)
            {
                return GameState.RedWon;
            }
            
            return GameState.Playing;
        }

        private void consume(Room room)
        {
            var foodToConsumeBlue = room.FoodList.FirstOrDefault(p => p.PositionX == room.BlueSnake.Head.PositionX && p.PositionY == room.BlueSnake.Head.PositionY);
            if (foodToConsumeBlue != null)
            {
                room.BlueSnake.Consume(foodToConsumeBlue);
                room.FoodList.Remove(foodToConsumeBlue);
            }

            var foodToConsumeRed = room.FoodList.FirstOrDefault(p => p.PositionX == room.RedSnake.Head.PositionX && p.PositionY == room.RedSnake.Head.PositionY);
            if (foodToConsumeRed != null)
            {
                room.RedSnake.Consume(foodToConsumeRed);
                room.FoodList.Remove(foodToConsumeRed);
            }

        }

    }
}
