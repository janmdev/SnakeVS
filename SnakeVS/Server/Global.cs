﻿using Microsoft.AspNetCore.SignalR;
using SnakeVS.Shared;

namespace SnakeVS.Server
{
    public static class Global
    {
        public static List<Room> Rooms = new List<Room>()
        {
            new Room()
            {
                Guid = Guid.NewGuid(),
                Name = "First room"
            }
        };

        public static IClientProxy ListingRoomsProxy;
        public static Dictionary<Guid, PlayerConnection> RoomClients = new Dictionary<Guid, PlayerConnection>();
    }
}
