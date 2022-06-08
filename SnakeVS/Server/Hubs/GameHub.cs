using Microsoft.AspNetCore.SignalR;
using MessagePack;
using SnakeVS.Shared;

namespace SnakeVS.Server.Hubs
{
    public class GameHub : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            await Clients.Caller.SendAsync("UpdateRoom", room);
        }

        public async Task SendMove(Guid roomGuid,Direction direction, Player player)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            if (player == Player.Blue)
            {
                room.BlueSnake.QueueDirection(direction);
            }

            if (player == Player.Red)
            {
                room.RedSnake.QueueDirection(direction);
            }
        }

        public async Task StartGame(Guid roomGuid)
        {
            var room = Global.Rooms.FirstOrDefault(p => p.Guid == roomGuid);
            room.State = GameState.Playing;
            gameRunner(room, Clients.Caller).Start();

        }

        private async Task gameRunner(Room room, IClientProxy client)
        {
            _ = FoodSpawner(room);
            while (true)
            {
                await room.BlueSnake.Move();
                await room.RedSnake.Move();
                consume(room);
                //room.State = checkGame(room);
                await client.SendAsync("UpdateRoom", room);
                //if (room.State != GameState.Playing) break;
                await Task.Delay(250);
            }
            //consume(snake1);
        }

        private async Task FoodSpawner(Room room)
        {
            while (true)
            {
                if (room.State != GameState.Playing) break;
                room.SpawnFood();
                await Task.Delay(3000);
            }
        }

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
