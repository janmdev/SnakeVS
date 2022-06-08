using MessagePack;
using SnakeVS.Shared;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var newRoom = new Room()
{
    Guid = Guid.NewGuid(),
    Name = "New room"
};
newRoom.SpawnFood();
newRoom.SpawnFood();
var serialized = MessagePackSerializer.Serialize(newRoom);
Console.WriteLine(serialized);
var room = MessagePackSerializer.Deserialize<Room>(serialized);

Console.WriteLine(room.Name);
