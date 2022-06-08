using MessagePack;

namespace SnakeVS.Shared
{
    [MessagePackObject]
    public class SnakeFood
    {
        [Key(30)]
        public sbyte PositionX { get; set; }
        [Key(31)]
        public sbyte PositionY { get; set; }
    }
}
