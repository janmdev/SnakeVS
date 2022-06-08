using MessagePack;
using SnakeVS.Shared;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var newRoom = new Room();

var serialized = MessagePackSerializer.Serialize(newRoom);
Console.WriteLine(serialized);
