using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace SnakeVS.Shared.Response
{
    [MessagePackObject]
    public class ListedRoom
    {
        public ListedRoom() {}
        public ListedRoom(Room room)
        {
            Id = room.Guid;
            Name = room.Name;
            int count = 0;
            if (!String.IsNullOrEmpty(room.BlueName)) count++;
            if (!String.IsNullOrEmpty(room.RedName)) count++;
            PlayerCount = count;
        }
        [Key(10)]
        public Guid Id { get; set; }
        [Key(11)]
        public string Name { get; set; }
        [Key(12)]
        public int PlayerCount { get; set; }
    }
}
